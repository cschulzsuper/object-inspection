using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Authorization;
using Super.Paula.Client.Authentication;
using Super.Paula.Client.Storage;
using Super.Paula.Environment;

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

            services.AddScoped<AuthenticationStateManager>(provider => new AuthenticationStateManager(
                provider.GetRequiredService<AppAuthentication>(),
                provider.GetRequiredService<ILocalStorage>(),
                new Lazy<IAccountHandler>(() => provider.GetRequiredService<IAccountHandler>())));

            services.AddScoped<AuthenticationStateProvider, AuthenticationStateManager>(provider
                 => provider.GetRequiredService<AuthenticationStateManager>());

            return services;
        }
    }
}