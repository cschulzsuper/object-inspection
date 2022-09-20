using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddBadgeAuthentication<
            TProofManager, TClaimsFilter, TClaimsFactory>

        (this IServiceCollection services, JsonSerializerOptions options)

        where TProofManager : class, IBadgeProofManager
        where TClaimsFilter : class, IBadgeClaimsFilter
        where TClaimsFactory : class, IBadgeClaimsFactory
    {
        services.AddAuthentication(setup =>
        {
            setup.DefaultScheme = "badge";
            setup.AddScheme<BadgeAuthenticationHandler>("badge", null);
        });

        services.AddBadgeHandler<TProofManager, TClaimsFilter, TClaimsFactory>(options);

        return services;
    }
}