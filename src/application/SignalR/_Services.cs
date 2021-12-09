using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Communication;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerSignalR(this IServiceCollection services)
            => services
                .AddScoped<HubContextResolver>()
                .AddScoped<INotificationMessenger, NotificationMessenger>();

    }
}
