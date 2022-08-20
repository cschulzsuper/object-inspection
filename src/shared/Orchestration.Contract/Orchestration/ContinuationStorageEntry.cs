using System.Security.Claims;

namespace Super.Paula.Shared.Orchestration;

public record ContinuationStorageEntry(ContinuationBase Continuation, ClaimsPrincipal User);