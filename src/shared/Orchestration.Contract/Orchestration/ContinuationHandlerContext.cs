using System;
using System.Security.Claims;

namespace Super.Paula.Shared.Orchestration;

public record ContinuationHandlerContext(
    IServiceProvider Services,
    ClaimsPrincipal User);