using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class InMemoryContinuationStorage : IContinuationStorage
    {
        private readonly ConcurrentDictionary<Guid, ContinuationStorageEntry> _continuations;
        private readonly ILogger<InMemoryContinuationStorage> _logger;

        public InMemoryContinuationStorage(ILogger<InMemoryContinuationStorage> logger)
        {
            _continuations = new ConcurrentDictionary<Guid, ContinuationStorageEntry>();
            _logger  =logger;
        }

        public ValueTask AddAsync(ContinuationBase continuation, ClaimsPrincipal user)
        {
            var entry = new ContinuationStorageEntry(continuation, user);

            _continuations.AddOrUpdate(continuation.Id, entry, (_,_) => entry);

            _logger.LogInformation("A continuation has been added ({continuation}, {user})", continuation, user);
            return ValueTask.CompletedTask;
        }

        public async IAsyncEnumerable<ContinuationStorageEntry> GetPendingContinuationsAsync()
        {
            await Task.CompletedTask;

            foreach(var continuation in _continuations.Values
                .Where(x => x.Continuation.Predecessors == Guid.Empty))
            {
                yield return continuation;
            }
        }

        public ValueTask SetContinuationAsInProgressAsync(Guid continuationId)
        {
            var found = _continuations.TryGetValue(continuationId, out var entry);

            if (found && entry != null)
            {
                _logger.LogInformation("A continuation has been marked as in progress ({continuation}, {user})", entry.Continuation, entry.User);
            }
            else
            {
                _logger.LogWarning("An unkown continuation has been marked as in progress ({continuationId})", continuationId);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask SetContinuationCompletionAsync(Guid continuationId)
        {
            var removed = _continuations.TryRemove(continuationId, out var entry);

            if (removed && entry != null)
            {
                _logger.LogInformation("A continuation has been marked as completed ({continuation}, {user})", entry.Continuation, entry.User);
            }
            else
            {
                _logger.LogWarning("A unkown continuation has been marked as completed ({continuationId})", continuationId);
            }

            var successors = _continuations.Values
                .Where(x => x.Continuation.Predecessors == continuationId);

            foreach(var succesor in successors)
            {
                succesor.Continuation.Predecessors = Guid.Empty;
            }

            return ValueTask.CompletedTask;
        }

        public async ValueTask SetContinuationFailureAsync(Guid continuationId, Exception? exception)
        {
            var removed = _continuations.TryRemove(continuationId, out var entry);

            if (removed && entry != null)
            {
                _logger.LogWarning(exception, "An continuation has been marked as failed ({continuation}, {user})", entry.Continuation, entry.User);
               
            }
            else
            {
                _logger.LogWarning(exception, "An unkown continuation has been marked as failed ({continuationId})", continuationId);
            }

            var successors = _continuations
                .Where(x => x.Value.Continuation.Predecessors == continuationId)
                .Select(x => x.Key)
                .ToList();

            foreach (var succesor in successors)
            {
                await SetContinuationFailureAsync(succesor, null);
            }
        }
    }
}
