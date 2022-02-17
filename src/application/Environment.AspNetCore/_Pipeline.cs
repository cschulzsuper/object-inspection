using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Authorization;
using Super.Paula.Environment;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Pipeline
    {
        public static IApplicationBuilder UsePaulaAppState(this IApplicationBuilder app)
            => app.Use((context, next) =>
                 {
                     var appState = context.RequestServices.GetRequiredService<AppState>();

                     appState.CurrentOrganization = context.User.HasOrganization()
                        ? context.User.GetOrganization()
                        : string.Empty;

                     appState.CurrentInspector = context.User.HasInspector()
                        ? context.User.GetInspector()
                        : string.Empty;

                     var endpoint = context.GetEndpoint();
                     var ignoreCurrentOrganization = endpoint?.Metadata.GetMetadata<IgnoreCurrentOrganizationAttribute>() != null;

                     appState.IgnoreCurrentOrganization = ignoreCurrentOrganization;

                     return next();

                 });
    }
}