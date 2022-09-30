using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record ContinuationStorageEntry(ContinuationBase Continuation, ClaimsPrincipal User);