using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Authorization
{
    public class PaulaAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(RequestDelegate requestDelegate, HttpContext context, 
            AuthorizationPolicy authorizationPolicy, PolicyAuthorizationResult authorizationResult)
        {
            if (authorizationResult.Forbidden &&
                authorizationPolicy.Requirements.Any(x => x is InspectorAuthorizationRequirement))
            {
                var authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(context.User, string.Empty));

                var policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();
                
                var hasInspector = context.Request.RouteValues.ContainsKey("inspector");
                
                if (hasInspector)
                {
                    var inspector = context.Request.RouteValues.Single(x => x.Key == "inspector");
                    authorizationResult = await policyEvaluator.AuthorizeAsync(authorizationPolicy, authenticateResult, context, inspector);
                }
            }

            await DefaultHandler.HandleAsync(requestDelegate, context, authorizationPolicy, authorizationResult);
        }
    }
}
