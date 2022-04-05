using System;
using System.Security.Claims;

namespace Super.Paula.Application.Orchestration
{
    public record ContinuationHandlerContext(
        IServiceProvider Services,
        ClaimsPrincipal User);
}
