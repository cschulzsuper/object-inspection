using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Authorization
{
    public class InspectorClaimResourceHandler : AuthorizationHandler<InspectorClaimResourceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, InspectorClaimResourceRequirement requirement)
        {
            var isAuthorized = context.User.HasAuthorizations(requirement.Authorizations);

            if (isAuthorized)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.Resource is KeyValuePair<string, object> resource &&
                resource.Key == "inspector")
            {
                var inspectorClaim = context.User.FindFirst("Inspector");

                if (inspectorClaim != null &&
                    inspectorClaim.Value.Equals(resource.Value))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}