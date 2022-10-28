using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using ChristianSchulz.ObjectInspection.BadgeUsage;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public class PersistentContinuationStorage : IContinuationStorage
{
    private readonly ILogger<PersistentContinuationStorage> _logger;
    private readonly IContinuationManager _continuationManager;
    private readonly IContinuationRegistry _continuationRegistry;
    private readonly IBadgeEncoding _badgeEncoding;
    private readonly ContinuationAwaiter _continuationAwaiter;

    public PersistentContinuationStorage(
        ILogger<PersistentContinuationStorage> logger,
        IContinuationManager continuationManager,
        IContinuationRegistry continuationRegistry,
        IBadgeEncoding badgeEncoding,
        ContinuationAwaiter continuationAwaiter)
    {
        _logger = logger;
        _continuationManager = continuationManager;
        _continuationRegistry = continuationRegistry;
        _badgeEncoding = badgeEncoding;
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
            Data = (char)0x00 + Base64Encoder.ObjectToBase64(continuation),
            User = (char)0x00 + _badgeEncoding.Encode(user.Claims),
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
            .ToList()
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

            var encodedData = continuation.Data.TrimStart((char)0x00); 
            var decodedData = (ContinuationBase)Base64Encoder.Base64ToObject(encodedData, continuationType);

            var encodedUser = continuation.User.TrimStart((char)0x00);
            var decodedUser = new ClaimsPrincipal(
                new ClaimsIdentity(
                    _badgeEncoding.Decode(encodedUser)));

            yield return (decodedData, decodedUser);
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