using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Shared.SignalR;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Shared;

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