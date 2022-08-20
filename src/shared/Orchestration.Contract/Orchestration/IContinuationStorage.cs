using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public interface IContinuationStorage
{
    ValueTask AddAsync(ContinuationBase continuation, ClaimsPrincipal user);

    IAsyncEnumerable<(ContinuationBase Continuation, ClaimsPrincipal User)> GetPendingContinuationsAsync();

    ValueTask<bool> SetContinuationAsInProgressAsync(Guid continuationId);

    ValueTask SetContinuationCompletionAsync(Guid continuationId);

    ValueTask SetContinuationFailureAsync(Guid continuationId, Exception? exception);
}