using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Super.Paula.RuntimeData
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerRuntimeData(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRuntimeCache<>), typeof(RuntimeCache<>));

            return services;
        }
    }
}
