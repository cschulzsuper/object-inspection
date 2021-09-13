using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Localization;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaClient(this IServiceCollection services, bool isDevelopment)
        {
            services.AddPaulaAppState();
            services.AddPaulaAppSettings();
            services.AddPaulaAppEnvironment(isDevelopment);
            services.AddPaulaClientTransport();
            services.AddPaulaClientAuthorization();

            services.AddSingleton<ITranslator,Translator>();

            return services;
        }
    }
}