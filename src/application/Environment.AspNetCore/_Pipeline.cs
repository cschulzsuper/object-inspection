using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
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
                    var bearer = context.User.FindFirst("Bearer")?.Value;

                    if (bearer != null)
                    {
                        var subject = Convert.FromBase64String(bearer);
                        var subjectValue = Encoding.UTF8.GetString(subject);
                        var subjectValues = subjectValue.Split(':', 3);

                        if (subjectValues?.Length == 3)
                        {
                            appAuthentication.Organization = subjectValues[0];
                            appAuthentication.Inspector = subjectValues[1];
                            appAuthentication.Bearer = bearer;

                            var accountHandler = context.RequestServices.GetRequiredService<IAccountHandler>();
                            appAuthentication.Authorizations = (await accountHandler.QueryAuthorizationsAsync())
                                .Values.ToArray();

                            var fallbackSubject = new byte[256];

                            if (Convert.TryFromBase64String(subjectValues[2], fallbackSubject, out var bytesWritten))
                            {
                                var fallbackSubjectValue = Encoding.UTF8.GetString(fallbackSubject);
                                if (fallbackSubjectValue.Count(x => x == ':') == 2)
                                {
                                    var fallbackSubjectValues = subjectValue.Split(':', 3);

                                    appAuthentication.ImpersonatorOrganization = fallbackSubjectValues[0];
                                    appAuthentication.ImpersonatorInspector = fallbackSubjectValues[1];
                                    appAuthentication.ImpersonatorBearer = subjectValues[2];
                                }
                            }
                        }
                    }
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