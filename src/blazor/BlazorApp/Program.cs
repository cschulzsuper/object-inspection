using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Client.Configuration;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.JSInterop;
using Super.Paula.Client.Storage;
using Microsoft.AspNetCore.Components.Web;

namespace Super.Paula.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.ConfigureServices(builder.HostEnvironment);

        var blazorApiUrl = builder.HostEnvironment.IsDevelopment()
                ? "http://localhost:7071"
                : builder.HostEnvironment.BaseAddress;

        await builder.Configuration.AddBlazorApiConfigurationAsync(blazorApiUrl);

        var host = builder.Build();

        CultureInfo culture;
        var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
        var cultureName = await jsRuntime.InvokeAsync<string?>("culture.get");

        if (cultureName != null)
        {
            culture = new CultureInfo(cultureName);
        }
        else
        {
            culture = new CultureInfo("en-US");
            await jsRuntime.InvokeVoidAsync("culture.set", "en-US");
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        await host.RunAsync();
    }

    public static void ConfigureServices(this IServiceCollection services, IWebAssemblyHostEnvironment environment)
    {
        services.AddBlazoredLocalStorage();
        services.AddClient<DefaultLocalStorage>(environment.IsDevelopment(), isWebAssembly: true);
    }
}