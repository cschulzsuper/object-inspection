using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Environment;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Pipeline
    {
        public static IApplicationBuilder UsePaulaAppAuthentication(this IApplicationBuilder app)
            => app.Use(async (context, next) =>
            {
                var appAuthentication = context.RequestServices.GetRequiredService<AppAuthentication>();
                
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var claim = (string type) => context.User.FindFirst(type)?.Value;

                    appAuthentication.Organization 
                        = claim("Organization") ?? appAuthentication.Organization;

                    appAuthentication.Inspector 
                        = claim("Inspector") ?? appAuthentication.Inspector;

                    appAuthentication.Proof 
                        = claim("Proof") ?? appAuthentication.Proof;

                    appAuthentication.ImpersonatorOrganization 
                        = claim("ImpersonatorOrganization") ?? appAuthentication.ImpersonatorOrganization;

                    appAuthentication.ImpersonatorInspector 
                        = claim("ImpersonatorInspector") ?? appAuthentication.ImpersonatorInspector;

                    var claims = (string type) => context.User.FindAll(type)
                        .Select(x => x.Value)
                        .ToArray();

                    var authorizations = claims("Authorization");

                    appAuthentication.Authorizations
                        = authorizations ?? appAuthentication.Authorizations;
                }

                await next();
            });

        public static IApplicationBuilder UsePaulaAppState(this IApplicationBuilder app)
            => app.Use((context, next) =>
                {
                    var appState = context.RequestServices.GetRequiredService<AppState>();
                    var appAuthentication = context.RequestServices.GetRequiredService<AppAuthentication>();

                    appState.CurrentOrganization = appAuthentication.Organization;
                    appState.CurrentInspector = appAuthentication.Inspector;

                    var endpoint = context.GetEndpoint();
                    var ignoreCurrentOrganization = endpoint?.Metadata.GetMetadata<IgnoreCurrentOrganizationAttribute>() != null;

                    appState.IgnoreCurrentOrganization = ignoreCurrentOrganization;

                    return next();

                });
    }
}