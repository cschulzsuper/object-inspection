using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Application;
using Super.Paula.Application.Orchestration;
using Super.Paula.Authentication;
using Super.Paula.Authorization;
using Super.Paula.BlobStorage;
using Super.Paula.Data;
using Super.Paula.Orchestration;
using Super.Paula.RuntimeData;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddServer(this IServiceCollection services, 
            IHostEnvironment environment, 
            IConfiguration configuration)
        {
            services.AddHostedService<WorkerService>();

            var isDevelopment = environment.IsDevelopment();
            var blobStorageConnectionString = configuration["Paula:BlobStorageConnectionString"] ?? string.Empty;

            services
                .AddAppEnvironment(isDevelopment)
                .AddAppSettings()
                .AddServerBlobStorage(blobStorageConnectionString)
                .AddServerData(isDevelopment)
                .AddServerRuntimeData()
                .AddServerManagement()
                .AddServerIntegration()
                .AddServerStreaming()
                .AddServerTransport()
                .AddServerAuthentication()
                .AddServerAuthorization();

            return services;
        }

        public static IServiceCollection AddServerAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "bearer";
                options.AddScheme<PaulaAuthenticationHandler>("bearer", null);
            });

            return services;
        }

        public static IServiceCollection AddServerAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            
            services
                .AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>()
                .AddScoped<IAuthorizationMiddlewareResultHandler, PaulaAuthorizationMiddlewareResultHandler>()
                .AddScoped<IAuthorizationHandler, AnyAuthorizationClaimHandler>()
                .AddScoped<IAuthorizationHandler, IdentityClaimResourceHandler>()
                .AddScoped<IAuthorizationHandler, InspectorClaimResourceHandler>()
                .AddUser();

            return services;
        }
    }
}