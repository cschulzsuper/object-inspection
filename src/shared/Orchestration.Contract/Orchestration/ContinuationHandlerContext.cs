using System;
using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record ContinuationHandlerContext(
    IServiceProvider Services,
    ClaimsPrincipal User);