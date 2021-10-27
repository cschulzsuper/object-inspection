using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Client.Configuration;

namespace Super.Paula.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.ConfigureServices(builder.HostEnvironment);

            var blazorApiUrl = builder.HostEnvironment.IsDevelopment()
                    ? "http://localhost:7071"
                    : builder.HostEnvironment.BaseAddress;

            await builder.Configuration.AddBlazorApiConfigurationAsync(blazorApiUrl);

            var host = builder.Build();
            await host.RunAsync();
        }

        public static void ConfigureServices(this IServiceCollection services, IWebAssemblyHostEnvironment environment)
        {
            services.AddPaulaClient(environment.IsDevelopment());
        }
    }
}
