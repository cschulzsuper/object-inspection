using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application;
using Super.Paula.Application.Operation;
using Super.Paula.Application.Orchestration;
using Super.Paula.Authorization;
using Super.Paula.Data;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Pipeline
    {
        public static IApplicationBuilder ConfigureEvents(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Configure((context) =>
            {
                var paulaContextState = context.Services.GetRequiredService<PaulaContextState>();

                paulaContextState.CurrentOrganization = context.User.HasOrganization()
                   ? context.User.GetOrganization()
                   : string.Empty;

                paulaContextState.CurrentInspector = context.User.HasInspector()
                   ? context.User.GetInspector()
                   : string.Empty;
            });

            eventBus.ConfigureTransport();

            return app;
        }

        public static IApplicationBuilder UseRequestUser(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.RequestServices.ConfigureUser(context.User);

                await next.Invoke(context);
            });

            return app;
        }

        public static IApplicationBuilder UseBlacklist(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.Use(async (context, next) =>
            {
                var connectionViolationManager = context.RequestServices.GetRequiredService<IConnectionViolationManager>();
                var connectionIpAddress = context.Connection.RemoteIpAddress?.ToString();

                var blacklisted =
                    connectionIpAddress != null &&
                    connectionViolationManager.Verify(connectionIpAddress);

                if (blacklisted)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }

                await next.Invoke(context);
            });

            return app;
        }

        public static IApplicationBuilder UseEndpointState(this IApplicationBuilder app)
            => app.Use((context, next) =>
            {
                var paulaContextState = context.RequestServices.GetRequiredService<PaulaContextState>();

                var endpoint = context.GetEndpoint();
                var useOrganizationFromRoute = endpoint?.Metadata.GetMetadata<UseOrganizationFromRouteAttribute>() != null;

                if (useOrganizationFromRoute)
                {
                    context.Request.RouteValues.TryGetValue("organization", out var organization);

                    paulaContextState.CurrentOrganization = organization != null
                        ? $"{organization}"
                        : string.Empty;

                    paulaContextState.CurrentInspector = string.Empty;
                }
                else
                {
                    paulaContextState.CurrentOrganization = context.User.HasOrganization()
                       ? context.User.GetOrganization()
                       : string.Empty;

                    paulaContextState.CurrentInspector = context.User.HasInspector()
                       ? context.User.GetInspector()
                       : string.Empty;
                }

                return next();

            });
    }
}