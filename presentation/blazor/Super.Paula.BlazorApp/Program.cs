using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Super.Paula.Environment;
using Super.Paula.Web.Client;
using Super.Paula.Web.Shared.Authorization;

namespace Super.Paula.BlazorApp
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
            services.AddPaulaWebClient(environment.IsDevelopment());
            services.AddPaulaWebClientAuthorization();
        }
    }
}
