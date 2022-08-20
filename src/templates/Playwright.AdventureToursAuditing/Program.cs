using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Shared;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.AdventureToursAuditing;

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

                services
                    .AddScoped<Steps.AdventureToursAuditing>();

            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddUserSecrets<Program>(true);
            });
}