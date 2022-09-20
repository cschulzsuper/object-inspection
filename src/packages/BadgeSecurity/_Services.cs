using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.BadgeUsage;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddBadgeHandler<
            TProofManager, TClaimsFilter, TClaimsFactory>
 
        (this IServiceCollection services, JsonSerializerOptions options)

        where TProofManager : class, IBadgeProofManager
        where TClaimsFilter : class, IBadgeClaimsFilter
        where TClaimsFactory : class, IBadgeClaimsFactory
    {
        services.AddBadgeEncoding(options);

        services.AddScoped<IBadgeHandler, BadgeHandler>();

        services.AddScoped<IBadgeProofManager, TProofManager>();
        services.AddScoped<IBadgeClaimsFilter, TClaimsFilter>();
        services.AddScoped<IBadgeClaimsFactory, TClaimsFactory>();

        return services;
    }
}