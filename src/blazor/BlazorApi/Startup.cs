using BlazorApi;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

[assembly: FunctionsStartup(typeof(Startup))]

namespace BlazorApi;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {

    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        var context = builder.GetContext();

        builder.ConfigurationBuilder.AddKeyPerFile(c =>
        {
            c.FileProvider = new PhysicalFileProvider(context.ApplicationRootPath);
            c.Optional = true;
            c.IgnoreCondition = fileName => !fileName.StartsWith("build__");
        });
    }
}