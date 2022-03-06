using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IContinuationStorage
    {
        ValueTask AddAsync(ContinuationBase continuation, ClaimsPrincipal user);

        IAsyncEnumerable<ContinuationStorageEntry> GetPendingContinuationsAsync();

        ValueTask SetContinuationAsInProgressAsync(Guid continuationId);

        ValueTask SetContinuationCompletionAsync(Guid continuationId);

        ValueTask SetContinuationFailureAsync(Guid continuationId, Exception? exception);
    }
}
