using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Data;
using Super.Paula.Migrator.Steps;
using Super.Paula.Shared;
using System.Threading.Tasks;

namespace Super.Paula.Migrator;

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
                services.AddUser();
                services.AddHostedService<HostedService>();
                services.AddAppSettings();
                services.AddServerData(context.HostingEnvironment.IsDevelopment());

                services
                    .AddScoped<Initialization>();

            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddUserSecrets<Program>(true);
            });
}