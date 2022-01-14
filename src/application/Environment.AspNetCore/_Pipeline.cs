using System.Diagnostics.CodeAnalysis;
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
                        = claim("organization") ?? appAuthentication.Organization;

                    appAuthentication.Inspector 
                        = claim("inspector") ?? appAuthentication.Inspector;

                    appAuthentication.Proof 
                        = claim("proof") ?? appAuthentication.Proof;

                    appAuthentication.ImpersonatorOrganization 
                        = claim("impersonatorOrganization") ?? appAuthentication.ImpersonatorOrganization;

                    appAuthentication.ImpersonatorInspector 
                        = claim("impersonatorInspector") ?? appAuthentication.ImpersonatorInspector;
                }

                await next();
            });

        public static IApplicationBuilder UsePaulaAppState(this IApplicationBuilder app)
            => app.Use((context, next) =>
                {
                    var appState = context.RequestServices.GetRequiredService<AppState>();
                    var appAuthentication = context.RequestServices.GetRequiredService<AppAuthentication>();

                    var endpoint = context.GetEndpoint();
                    var ignoreCurrentOrganization = endpoint?.Metadata.GetMetadata<IgnoreCurrentOrganizationAttribute>() != null;

                    appState.CurrentOrganization = appAuthentication.Organization;
                    appState.CurrentInspector = appAuthentication.Inspector;
                    appState.IgnoreCurrentOrganization = ignoreCurrentOrganization;

                    return next();

                });
    }
}