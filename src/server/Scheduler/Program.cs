using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Application;
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

                    services.AddUser();
                    services.AddPaulaAppSettings();

                    services.AddPaulaServerData(context.HostingEnvironment.IsDevelopment());
                    services.AddPaulaServerIntegration();
                    services.AddPaulaServerManagement();
                    services.AddPaulaServerTransport();

                    services.AddPaulaRemoteStreaming();
   
                    services
                        .AddScoped<BusinessObjectInspectionAuditScheduleCalculation>();

                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddUserSecrets<Program>(true);
                });
    }
}
