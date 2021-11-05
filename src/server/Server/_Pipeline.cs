using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Runtime;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Pipeline
    {
        public static IApplicationBuilder UsePaulaBlacklist(this IApplicationBuilder app)
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
    }
}