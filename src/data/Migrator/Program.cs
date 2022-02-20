﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Data.Steps;
using System.Threading.Tasks;

namespace Super.Paula.Data
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
                    services.AddUser();
                    services.AddHostedService<HostedService>();
                    services.AddPaulaAppSettings();
                    services.AddPaulaServerData(context.HostingEnvironment.IsDevelopment());

                    services
                        .AddScoped<Initialization>()
                        .AddScoped<InitializationApplication>();
                        //.AddScoped<InspectorIdentity>();

                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddUserSecrets<Program>(true);
                });
    }
}
