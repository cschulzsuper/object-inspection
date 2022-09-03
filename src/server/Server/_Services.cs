using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Application;
using Super.Paula.BlobStorage;
using Super.Paula.Data;
using Super.Paula.RuntimeData;
using Super.Paula.Server.Orchestration;
using Super.Paula.Shared;
using System.Diagnostics.CodeAnalysis;
using Super.Paula.Server.Security;
using Super.Paula.Shared.Security;

namespace Super.Paula.Server;

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
            .AddServerBroadcaster()
            .AddServerTransport()
            .AddServerAuthentication()
            .AddServerAuthorization();

        return services;
    }

    public static IServiceCollection AddServerAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "badge";
            options.AddScheme<BadgeAuthenticationHandler>("badge", null);
        });

        services.AddScoped<IBadgeAuthenticationTracker, BadgeAuthenticationTracker>();

        return services;
    }

    public static IServiceCollection AddServerAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        services
            .AddSingleton<IAuthorizationPolicyProvider, BadgeAuthorizationPolicyProvider>()
            .AddScoped<IAuthorizationMiddlewareResultHandler, BadgeAuthorizationMiddlewareResultHandler>()
            .AddScoped<IAuthorizationHandler, AnyAuthorizationClaimHandler>()
            .AddScoped<IAuthorizationHandler, IdentityClaimResourceHandler>()
            .AddScoped<IAuthorizationHandler, InspectorClaimResourceHandler>()
            .AddUser();

        return services;
    }
}