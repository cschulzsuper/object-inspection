﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Server.Security;

public class ResourceAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

    public async Task HandleAsync(RequestDelegate requestDelegate, HttpContext context,
        AuthorizationPolicy authorizationPolicy, PolicyAuthorizationResult authorizationResult)
    {
        if (authorizationResult.Forbidden &&
            authorizationPolicy.Requirements.Any(x =>
                x is InspectorClaimResourceRequirement ||
                x is IdentityClaimResourceRequirement))
        {
            var authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(context.User, string.Empty));

            var policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authorizationService = context.RequestServices.GetRequiredService<IAuthorizationService>();

            var hasItem =
                context.Request.RouteValues.ContainsKey("inspector") ||
                context.Request.RouteValues.ContainsKey("identity");

            if (hasItem)
            {
                var inspector = context.Request.RouteValues.Single(x => x.Key is "inspector" or "identity");

                // TODO something is wrong here why async and why _?
                _ = authorizationService.AuthorizeAsync(context.User, inspector, authorizationPolicy);

                authorizationResult = await policyEvaluator.AuthorizeAsync(authorizationPolicy, authenticateResult, context, inspector);
            }
        }

        await _defaultHandler.HandleAsync(requestDelegate, context, authorizationPolicy, authorizationResult);
    }
}