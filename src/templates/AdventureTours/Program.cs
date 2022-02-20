using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Data;
using Super.Paula.Steps;
using System.Threading.Tasks;

namespace Super.Paula
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
                    services.AddPaulaAppSettings();

                    services.AddPaulaServerData(context.HostingEnvironment.IsDevelopment());

                    services
                        .AddScoped<Purge>()
                        .AddScoped<Initialization>()
                        .AddScoped<InitializationAdministration>()
                        .AddScoped<InitializationAdventureTours>();

                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddUserSecrets<Program>(true);
                });
    }
}
