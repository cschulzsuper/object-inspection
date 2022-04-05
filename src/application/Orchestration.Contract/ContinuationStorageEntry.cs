using System.Security.Claims;

namespace Super.Paula.Application.Orchestration
{
    public record ContinuationStorageEntry(ContinuationBase Continuation, ClaimsPrincipal User);
}
