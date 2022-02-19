using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Super.Paula.Authorization
{
    public class AnyAuthorizationClaimHandler : AuthorizationHandler<AnyAuthorizationClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnyAuthorizationClaimRequirement requirement)
        {
            var isAuthorized = context.User
                .HasAuthorizations(requirement.Authorizations);

            if (isAuthorized)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}