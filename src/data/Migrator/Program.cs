using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Data.Steps;

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
                    services.AddHostedService<HostedService>();
                    services.AddPaulaAppState();
                    services.AddPaulaAppSettings();
                    services.AddPaulaServerData(context.HostingEnvironment.IsDevelopment());

                    services
                        .AddScoped<Initialization>()
                        .AddScoped<BusinessObjectInspectionAuditSchedule>()
                        .AddScoped<InspectorBusinessObjects>();

                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddUserSecrets<Program>(true);
                });
    }
}
