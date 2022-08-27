using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Application.Orchestration;
using Super.Paula.Shared.Security;

namespace Super.Paula.Shared.Orchestration;

public class PersistentContinuationStorage : IContinuationStorage
{
    private readonly ILogger<PersistentContinuationStorage> _logger;
    private readonly IContinuationManager _continuationManager;
    private readonly IContinuationRegistry _continuationRegistry;
    private readonly ContinuationAwaiter _continuationAwaiter;

    public PersistentContinuationStorage(
        ILogger<PersistentContinuationStorage> logger,
        IContinuationManager continuationManager,
        IContinuationRegistry continuationRegistry,
        ContinuationAwaiter continuationAwaiter)
    {
        _logger = logger;
        _continuationManager = continuationManager;
        _continuationRegistry = continuationRegistry;
        _continuationAwaiter = continuationAwaiter;
    }

    public async ValueTask AddAsync(ContinuationBase continuation, ClaimsPrincipal user)
    {
        var entity = new Continuation
        {
            Name = TypeNameConverter.ToKebabCase(continuation.GetType()),
            CreationTime = continuation.CreationTime,
            CreationDate = continuation.CreationDate,
            ContinuationId = continuation.Id.ToString(),
            Data = Base64Encoder.ObjectToBase64(continuation),
            User = Base64Encoder.ObjectToBase64(user.ToToken()),
            OperationId = Activity.Current?.RootId ?? Guid.NewGuid().ToString(),
        };

        await _continuationManager.InsertAsync(entity);

        _logger.LogInformation("A continuation has been added ({continuation}, {user})", continuation, user);

        _continuationAwaiter.Signal();
    }

    public async IAsyncEnumerable<(ContinuationBase, ClaimsPrincipal)> GetPendingContinuationsAsync()
    {
        await _continuationAwaiter.WaitAsync();

        // As soon as the ef core cosmos db provider can create composite indices this can be refactored.
        // The refactoring will put the order by into a call of GetAsyncEnumerable().
        //
        // https://github.com/dotnet/efcore/issues/17303 

        var continuations = _continuationManager
            .GetQueryable()
            .Where(x => x.State == string.Empty)
            .AsEnumerable()
            .OrderBy(x => x.CreationDate)
            .ThenBy(x => x.CreationTime);

        foreach (var continuation in continuations)
        {
            await Task.CompletedTask;

            var continuationType = _continuationRegistry.GetContinuationType(continuation.Name);

            if (continuationType == null)
            {
                _logger.LogWarning("Could not get continuation type for a continuation ({continuation}).", continuation);
                continue;
            }

            var data = (ContinuationBase)Base64Encoder.Base64ToObject(continuation.Data, continuationType);

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    Base64Encoder.Base64ToObject<Token>(continuation.User).ToClaims()));

            yield return (data, user);
        }
    }

    public async ValueTask<bool> SetContinuationAsInProgressAsync(Guid continuationId)
    {
        var continuation = _continuationManager.GetQueryable()
            .SingleOrDefault(x => x.ContinuationId == continuationId.ToString());

        if (continuation is not null and not { State: "in-progress" })
        {
            try
            {
                continuation.State = "in-progress";
                await _continuationManager.UpdateAsync(continuation);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "An error occured while marking continuation as in progress ({continuationId})", continuationId);
                return false;
            }

            _logger.LogInformation("A continuation has been marked as in progress ({continuationId})", continuationId);
            return true;
        }
        else
        {
            _logger.LogWarning("Could not mark continuation as in progress ({continuationId})", continuationId);
            return false;
        }
    }

    public async ValueTask SetContinuationCompletionAsync(Guid continuationId)
    {
        var continuation = _continuationManager.GetQueryable()
            .SingleOrDefault(x => x.ContinuationId == continuationId.ToString());

        if (continuation is { State: "in-progress" })
        {
            await _continuationManager.DeleteAsync(continuation);

            _logger.LogInformation("A continuation has been marked as completed ({continuationId})", continuationId);
        }
        else
        {
            _logger.LogWarning("Could not mark continuation as completed ({continuationId})", continuationId);
        }
    }

    public async ValueTask SetContinuationFailureAsync(Guid continuationId, Exception? exception)
    {
        var continuation = _continuationManager.GetQueryable()
            .SingleOrDefault(x => x.ContinuationId == continuationId.ToString());

        if (continuation is { State: "in-progress" })
        {
            continuation.State = "failed";
            await _continuationManager.UpdateAsync(continuation);

            _logger.LogWarning(exception, "An continuation has been marked as failed ({continuationId})", continuationId);

        }
        else
        {
            _logger.LogWarning(exception, "Could not mark continuation as failed ({continuationId})", continuationId);
        }
    }
}