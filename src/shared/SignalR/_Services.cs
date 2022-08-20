using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Shared.SignalR;
using System.Linq;

namespace Super.Paula.Shared;

public static class _Services
{
    public static IServiceCollection AddServerSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/octet-stream" });
        });

        services.AddScoped<HubContextResolver>();

        return services;
    }
}