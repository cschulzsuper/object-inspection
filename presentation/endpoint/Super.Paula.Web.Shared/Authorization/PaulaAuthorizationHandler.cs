﻿using Microsoft.AspNetCore.Authorization;
using Super.Paula.Web.Shared.Authentication;
using Super.Paula.Web.Shared.Handling;
using System.Threading.Tasks;

namespace Super.Paula.Web.Shared.Authorization
{
    public class PaulaAuthorizationHandler : AuthorizationHandler<PaulaAuthorizationRequirement>
    {
        private readonly IAccountHandler _accountHandler;

        public PaulaAuthorizationHandler(IAccountHandler accountHandler)
        {
            _accountHandler = accountHandler;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PaulaAuthorizationRequirement requirement)
        {
            var authorizations = (await _accountHandler.QueryAuthorizationsAsync()).Values;

            if (string.IsNullOrWhiteSpace(requirement.Value) &&
                authorizations.Any())
            {
                context.Succeed(requirement);
            }

            if (authorizations.Contains(requirement.Value))
            {
                context.Succeed(requirement);
            }
        }
    }
}