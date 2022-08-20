using System;
using System.Security.Claims;

namespace Super.Paula.Shared.Orchestration;

public record EventHandlerContext(
    IServiceProvider Services,
    ClaimsPrincipal User);