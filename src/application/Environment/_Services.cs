using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Environment;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
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
                        MaintainerIdentity = configuration["MaintainerIdentity"],
                        DemoIdentity = configuration["DemoIdentity"],
                        Server = configuration["Server"],
                        Client = configuration["Client"],
                    };
                });
    }
}
