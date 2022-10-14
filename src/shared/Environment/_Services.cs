using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using ChristianSchulz.ObjectInspection.Shared.Environment;

namespace ChristianSchulz.ObjectInspection.Shared;

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
                    SqlServer = configuration["ObjectInspection:SqlServer"] ?? string.Empty,
                    MaintainerIdentity = configuration["ObjectInspection:MaintainerIdentity"] ?? string.Empty,
                    DemoIdentity = configuration["ObjectInspection:DemoIdentity"] ?? string.Empty,
                    DemoPassword = configuration["ObjectInspection:DemoPassword"] ?? string.Empty,
                    Server = configuration["ObjectInspection:Server"] ?? string.Empty,
                    Client = configuration["ObjectInspection:Client"] ?? string.Empty
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