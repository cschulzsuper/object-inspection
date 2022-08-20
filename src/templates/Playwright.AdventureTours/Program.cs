﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Data;
using Super.Paula.Shared;
using Super.Paula.Templates.Playwright.AdventureTours.Environment;
using Super.Paula.Templates.Playwright.AdventureTours.Steps;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.AdventureTours;

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
                    .AddScoped<InitializationAdministration>()
                    .AddScoped<InitializationAdventureTours>();

            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddEnvironmentVariables();
                builder.AddUserSecrets<Program>(true);
            });
}