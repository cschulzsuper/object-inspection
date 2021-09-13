using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;
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
            => app.Use((context, next) =>
            {
                var appAuthentication = context.RequestServices.GetRequiredService<AppAuthentication>();

                var authorizationHeader = context.Request.Headers.Authorization.ToString();

                if (authorizationHeader != null &&
                    authorizationHeader.StartsWith("Bearer "))
                {
                    var bearer = authorizationHeader.Replace("Bearer ", string.Empty);

                    appAuthentication.Bearer = bearer;

                    var subject = Convert.FromBase64String(bearer);
                    var subjectValue = Encoding.UTF8.GetString(subject);
                    var subjectValues = subjectValue.Split(':', 3);

                    if (subjectValues.Length == 3)
                    {
                        var fallbackSubject = new Span<byte>(new byte[256]);

                        if (Convert.TryFromBase64String(subjectValues[2], fallbackSubject, out var bytesWritten))
                        {
                            var fallbackSubjectValue = Encoding.UTF8.GetString(fallbackSubject);
                            if (fallbackSubjectValue.Count( x => x == ':') == 2)
                            {
                                appAuthentication.Impersonator = subjectValues[2];
                            }
                        }
                    }
                }

                return next();
            });

        public static IApplicationBuilder UsePaulaAppState(this IApplicationBuilder app)
            => app.Use((context, next) =>
                {
                    var currentContext = context.RequestServices.GetRequiredService<AppState>();

                    if (context.User.Identity?.IsAuthenticated == true)
                    {
                        var subjectValues = context.User
                            .FindFirst(ClaimTypes.NameIdentifier)?.Value
                            .Split(':', 3); ;

                        if (subjectValues != null)
                        {
                            currentContext.CurrentOrganization = subjectValues[0];
                            currentContext.CurrentInspector = subjectValues[1];
                        }
                    }

                    var endpoint = context.GetEndpoint();
                    var ignoreCurrentOrganization = endpoint?.Metadata.GetMetadata<IgnoreCurrentOrganizationAttribute>() != null;

                    currentContext.IgnoreCurrentOrganization = ignoreCurrentOrganization;

                    return next();

                });
    }
}