using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Data;
using Super.Paula.Templates.AdventureTours.Environment;
using Super.Paula.Templates.AdventureTours.Steps;
using System.Threading.Tasks;

namespace Super.Paula.Templates.AdventureTours
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<HostedService>();
                    services.AddAppSettings();

                    services.AddSingleton(serviceProvider =>
                    {
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                        return new AdventureToursSettings
                        {
                            MaintainerPassword = configuration["Paula:MaintainerPassword"] ?? string.Empty,
                            ChiefPassword = configuration["Paula:ChiefPassword"] ?? string.Empty
                        };
                    });

                    services.AddServerData(context.HostingEnvironment.IsDevelopment());

                    services
                        .AddScoped<Purge>()
                        .AddScoped<Initialization>()
                        .AddScoped<InitializationAdministration>()
                        .AddScoped<InitializationAdventureTours>();

                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddEnvironmentVariables();
                    builder.AddUserSecrets<Program>(true);
                });
    }
}
