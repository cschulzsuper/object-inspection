using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Super.Paula.Environment.AspNetCore;
using Super.Paula.Web.Server;
using Super.Paula.Web.Server.Authentication;
using Super.Paula.Web.Shared.Authorization;
using Super.Paula.Web.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using Super.Paula.Data.AspNetCore;

namespace Super.Paula.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddSwaggerGen();

            services.AddPaulaWebServer(_environment.IsDevelopment());
            services.AddPaulaWebServerAuthentication();
            services.AddPaulaWebServerAuthorization();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.EnsurePaulaData();                

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Super.Paula.Endpoints v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UsePaulaAppAuthentication();
            app.UsePaulaAppState();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPaulaWebServer();
                endpoints.MapGet("/hello-world", () => "hello world").RequireAuthorization().WithMetadata(new DisplayNameAttribute());
            });
        }     
    }
}