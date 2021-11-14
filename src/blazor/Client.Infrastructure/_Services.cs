using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Authorization;
using Super.Paula.Client.Authentication;

namespace Super.Paula.Client
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaClientAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PaulaAuthorizationHandler>();

            services.AddScoped<AuthenticationStateManager>();
            services.AddScoped<AuthenticationStateProvider, AuthenticationStateManager>(provider
                 => provider.GetRequiredService<AuthenticationStateManager>());

            return services;
        }
    }
}