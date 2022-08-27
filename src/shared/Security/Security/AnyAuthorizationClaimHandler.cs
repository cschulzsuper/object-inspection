using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Shared.Security;

public class AnyAuthorizationClaimHandler : AuthorizationHandler<AnyAuthorizationClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnyAuthorizationClaimRequirement requirement)
    {
        var isAuthorized = context.User
            .HasAnyAuthorization(requirement.Authorizations);

        if (isAuthorized)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}