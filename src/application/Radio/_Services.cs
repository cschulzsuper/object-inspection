using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Communication;
using Super.Paula.Shared;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerBroadcaster(this IServiceCollection services)
    {
        services.AddServerSignalR();

        services
            .AddScoped<IInspectorBroadcaster, InspectorBroadcaster>()
            .AddScoped<INotificationBroadcaster, NotificationBroadcaster>();

        return services;
    }
}