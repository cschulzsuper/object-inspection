using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Super.Paula
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.ConfigureServices(builder.HostEnvironment);

            var host = builder.Build();
            await host.RunAsync();
        }

        public static void ConfigureServices(this IServiceCollection services, IWebAssemblyHostEnvironment environment)
        {
            services.AddPaulaClient(environment.IsDevelopment());
        }
    }
}
