using System;
using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record EventHandlerContext(
    IServiceProvider Services,
    ClaimsPrincipal User);