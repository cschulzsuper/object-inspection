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
        public static IServiceCollection AddPaulaClientAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PaulaAuthorizationHandler>();

            services.AddScoped<PaulaAuthenticationStateManager>();
            services.AddScoped<AuthenticationStateProvider, PaulaAuthenticationStateManager>(provider
                 => provider.GetRequiredService<PaulaAuthenticationStateManager>());

            return services;
        }
    }
}