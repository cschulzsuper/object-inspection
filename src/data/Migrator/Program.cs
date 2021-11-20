using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migrator.Steps;
using Super.Paula;
using Super.Paula.Data;
using Super.Paula.Environment;
using System.Threading.Tasks;

namespace Super.Playground.Data.Migrator
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
                    services.AddPaulaAppState();
                    services.AddPaulaAppSettings();
                    services.AddPaulaServerData(context.HostingEnvironment.IsDevelopment());

                    services
                        .AddScoped<Initialization>();

                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddUserSecrets<Program>(true);
                });
    }
}
