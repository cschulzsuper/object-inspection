using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChristianSchulz.ObjectInspection.Data;
using ChristianSchulz.ObjectInspection.Migrator.Steps;
using ChristianSchulz.ObjectInspection.Shared;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Migrator;

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