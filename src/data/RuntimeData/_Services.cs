using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.RuntimeData;

namespace Super.Paula.Data
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
