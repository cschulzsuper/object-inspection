using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChristianSchulz.ObjectInspection.Application;
using ChristianSchulz.ObjectInspection.BlobStorage;
using ChristianSchulz.ObjectInspection.Data;
using ChristianSchulz.ObjectInspection.RuntimeData;
using ChristianSchulz.ObjectInspection.Server.Orchestration;
using ChristianSchulz.ObjectInspection.Shared;
using System.Diagnostics.CodeAnalysis;
using ChristianSchulz.ObjectInspection.BadgeSecurity;
using ChristianSchulz.ObjectInspection.Server.Security;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Server;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServer(this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.AddHostedService<WorkerService>();

        var isDevelopment = environment.IsDevelopment();
        var blobStorageConnectionString = configuration["ObjectInspection:BlobStorageConnectionString"] ?? string.Empty;

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
        services.AddBadgeAuthentication<BadgeProofManager, BadgeClaimsFilter, BadgeClaimsFactory>(ClaimsJsonSerializerOptions.Options);

        return services;
    }

    public static IServiceCollection AddServerAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        services
            .AddSingleton<IAuthorizationPolicyProvider, BadgeAuthorizationPolicyProvider>()
            .AddScoped<IAuthorizationMiddlewareResultHandler, ResourceAuthorizationMiddlewareResultHandler>()
            .AddScoped<IAuthorizationHandler, AnyAuthorizationClaimHandler>()
            .AddScoped<IAuthorizationHandler, IdentityClaimResourceHandler>()
            .AddScoped<IAuthorizationHandler, InspectorClaimResourceHandler>()
            .AddUser();

        return services;
    }
}