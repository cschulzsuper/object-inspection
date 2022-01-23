using Microsoft.AspNetCore.Authorization;
using Super.Paula.Environment;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Authorization
{
    public class PaulaAuthorizationHandler : AuthorizationHandler<PaulaAuthorizationRequirement>
    {
        private readonly AppAuthentication _appAuthentication;

        public PaulaAuthorizationHandler(AppAuthentication appAuthentication)
        {
            _appAuthentication = appAuthentication;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PaulaAuthorizationRequirement requirement)
        {
            var authorizations = _appAuthentication.Authorizations
                .Where(x =>
                        _appAuthentication.AuthorizationsFilter.Any() == false ||
                        _appAuthentication.AuthorizationsFilter.Contains(x))
                .ToArray();

            if (!authorizations.Any())
            {
                var claims = (string type) => context.User.FindAll(type)
                    .Select(x => x.Value)
                    .ToArray();

                authorizations = claims("Authorization");
            }

            if (string.IsNullOrWhiteSpace(requirement.Value) &&
                authorizations.Any())
            {
                context.Succeed(requirement);
            }

            if (authorizations.Contains(requirement.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}