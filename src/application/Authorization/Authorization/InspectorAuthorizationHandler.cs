using Microsoft.AspNetCore.Authorization;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Super.Paula.Authorization
{
    public class InspectorAuthorizationHandler : AuthorizationHandler<InspectorAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, InspectorAuthorizationRequirement requirement)
        {
            if (context.Resource is KeyValuePair<string, string> resource &&
                resource.Key == "inspector")
            {
                var inspectorClaim = context.User.FindFirst("inspector");

                if (inspectorClaim != null &&
                    inspectorClaim.Value == resource.Value)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}