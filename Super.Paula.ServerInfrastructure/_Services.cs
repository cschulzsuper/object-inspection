using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Authentication;
using Super.Paula.Authorization;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
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

            services.AddScoped<PaulaAuthenticationStateManager>();
            services.AddScoped<AuthenticationStateProvider,PaulaAuthenticationStateManager>(provider 
                => provider.GetRequiredService<PaulaAuthenticationStateManager>());

            return services;
        }
    }
}