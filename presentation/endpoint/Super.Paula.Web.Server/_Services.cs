using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServer(this IServiceCollection services, bool isDevlopment)
        {
            services
                .AddPaulaAppAuthentication()
                .AddPaulaAppEnvironment(isDevlopment)
                .AddPaulaAppSettings()
                .AddPaulaAppState()
                .AddPaulaData()
                .AddPaulaManagement()
                .AddPaulaServerTransport()
                .AddPaulaServerAuthentication()
                .AddPaulaServerAuthorization();

            return services;
        }
    }
}