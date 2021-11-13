using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application;
using Super.Paula.Authentication;
using Super.Paula.Authorization;
using Super.Paula.Data;

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
                .AddPaulaServerData(isDevlopment)
                .AddPaulaServerRuntimeData()
                .AddPaulaServerManagement()
                .AddPaulaServerSignalR()
                .AddPaulaServerTransport()
                .AddPaulaServerAuthentication()
                .AddPaulaServerAuthorization();

            return services;
        }

        public static IServiceCollection AddPaulaServerAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "bearer";
                options.AddScheme<PaulaAuthenticationHandler>("bearer", null);
            });

            return services;
        }

        public static IServiceCollection AddPaulaServerAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PaulaAuthorizationHandler>();

            return services;
        }
    }
}