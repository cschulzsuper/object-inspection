using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Environment;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaAppState(this IServiceCollection services)
            => services.AddScoped<AppState>();

        public static IServiceCollection AddPaulaAppAuthentication(this IServiceCollection services)
            => services.AddScoped<AppAuthentication>();

        public static IServiceCollection AddPaulaAppEnvironment(this IServiceCollection services, bool isDevelopment)
            => services.AddSingleton(_ =>
                new AppEnvironment
                {
                    IsDevelopment = isDevelopment
                });

        public static IServiceCollection AddPaulaAppSettings(this IServiceCollection services)
            => services.AddSingleton(serviceProvider =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                    return new AppSettings
                    {
                        StreamerSecret = configuration["StreamerSecret"],
                        CosmosEndpoint = configuration["CosmosEndpoint"],
                        CosmosKey = configuration["CosmosKey"],
                        CosmosDatabase = configuration["CosmosDatabase"],
                        Maintainer = configuration["Maintainer"],
                        MaintainerOrganization = configuration["MaintainerOrganization"],
                        DemoInspector = configuration["DemoInspector"],
                        DemoOrganization = configuration["DemoOrganization"],
                        Server = configuration["Server"]
                    };
                });
    }
}
