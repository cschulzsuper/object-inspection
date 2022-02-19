using System;
using System.Security.Claims;

namespace Super.Paula.Application.Orchestration
{
    public record EventHandlerContext(
        IServiceProvider Services,
        ClaimsPrincipal User,
        Attribute[] Annotations);
}
