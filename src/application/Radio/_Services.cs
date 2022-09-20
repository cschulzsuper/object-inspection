using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Communication;
using ChristianSchulz.ObjectInspection.Shared;
using System.Diagnostics.CodeAnalysis;
using ChristianSchulz.ObjectInspection.Application.SignalR;

namespace ChristianSchulz.ObjectInspection.Application;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerBroadcaster(this IServiceCollection services)
    {
        services.AddServerSignalR();

        services.AddSingleton<IUserIdProvider, RadioUserIdProvider>();

        services
            .AddScoped<IInspectorBroadcaster, InspectorBroadcaster>()
            .AddScoped<INotificationBroadcaster, NotificationBroadcaster>();

        return services;
    }
}