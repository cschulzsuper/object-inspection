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
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Super.Paula.Web.Shared.Handling.Responses;

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

            app.UseExceptionHandler(appBuilder => appBuilder.Run(HandleError));

            if (_environment.IsDevelopment())
            {
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
            });
        }

        public async Task HandleError(HttpContext context)
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path
                });
                return;
            }

            var exception = exceptionHandlerPathFeature.Error;

            var httpMethod = context.Request.Method;
            var statusCode = httpMethod switch
            {
                _ when HttpMethods.IsGet(httpMethod) => StatusCodes.Status404NotFound,
                _ when HttpMethods.IsHead(httpMethod) => StatusCodes.Status404NotFound,
                _ when HttpMethods.IsPost(httpMethod) => StatusCodes.Status400BadRequest,
                _ when HttpMethods.IsPut(httpMethod) => StatusCodes.Status400BadRequest,
                _ when HttpMethods.IsPatch(httpMethod) => StatusCodes.Status400BadRequest,
                _ when HttpMethods.IsDelete(httpMethod) => StatusCodes.Status400BadRequest,
                _ => throw exception
            };

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync( 
                new _ProblemDetails {
                    Detail = exception.StackTrace ?? string.Empty,
                    Title = exception.Message,
                    Status = statusCode,
                    Instance = context.Request.Path});
        }
    }
}