using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Communication;
using Super.Paula.Environment;
using Super.Paula.SignalR;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaSignalR(this IServiceCollection services)
            => services
                .AddScoped<HubContextResolver>()
                .AddScoped<INotificationMessenger, NotificationMessenger>();

    }
}
