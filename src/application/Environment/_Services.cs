using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Environment;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddAppEnvironment(this IServiceCollection services, bool isDevelopment)
            => services.AddSingleton(_ =>
                new AppEnvironment
                {
                    IsDevelopment = isDevelopment
                });

        public static IServiceCollection AddAppSettings(this IServiceCollection services)
            => services.AddSingleton(serviceProvider =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                    return new AppSettings
                    {
                        StreamerSecret = configuration["Paula:StreamerSecret"],
                        CosmosEndpoint = configuration["Paula:CosmosEndpoint"],
                        CosmosKey = configuration["Paula:CosmosKey"],
                        CosmosDatabase = configuration["Paula:CosmosDatabase"],
                        MaintainerIdentity = configuration["Paula:MaintainerIdentity"],
                        DemoIdentity = configuration["Paula:DemoIdentity"],
                        DemoPassword = configuration["Paula:DemoPassword"],
                        Server = configuration["Paula:Server"],
                        Client = configuration["Paula:Client"]
                    };
                });

        public static IServiceCollection AddBuildInfo(this IServiceCollection services)
            => services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                return new BuildInfo
                {
                    Branch = (configuration["Build:Branch"] ?? string.Empty).Trim(),
                    Hash = (configuration["Build:Hash"] ?? string.Empty).Trim(),
                    ShortHash = (configuration["Build:ShortHash"] ?? string.Empty).Trim(),
                    Build = (configuration["Build:Build"] ?? string.Empty).Trim(),
                    Runtime = RuntimeInformation.FrameworkDescription
                };
            });
    }
}
