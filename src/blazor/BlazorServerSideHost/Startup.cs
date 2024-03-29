using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChristianSchulz.ObjectInspection.Client.Storage;

namespace ChristianSchulz.ObjectInspection.Client;

public class Startup
{
    private readonly IWebHostEnvironment _environment;

    public Startup(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();

        services.AddClient<ProtectedLocalStorage>(_environment.IsDevelopment(), isWebAssembly: false);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();

        if (_environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }
}