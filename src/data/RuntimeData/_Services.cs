using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.RuntimeData;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerRuntimeData(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IRuntimeCache<>), typeof(RuntimeCache<>));

        return services;
    }
}