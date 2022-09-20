using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace ChristianSchulz.ObjectInspection.BadgeUsage;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddBadgeEncoding(this IServiceCollection services, JsonSerializerOptions options)
    {
        services.AddSingleton<IBadgeEncoding>(_ => new BadgeEncoding(options));
        return services;
    }
}