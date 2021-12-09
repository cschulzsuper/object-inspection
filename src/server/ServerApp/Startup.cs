using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Super.Paula.Application.Runtime;
using Super.Paula.Swagger;
using Super.Paula.Validation;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ProblemDetails = Super.Paula.ErrorHandling.ProblemDetails;

namespace Super.Paula
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddSwaggerGen();
            
            services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddCors(options => 
                options.AddDefaultPolicy(policy => 
                    policy
                        .WithOrigins(_configuration["BlazorApp"])
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            services.AddPaulaServer(_environment.IsDevelopment());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UsePaulaBlacklist();

            app.UseResponseCompression();
            app.UseExceptionHandler(appBuilder => appBuilder.Run(HandleError));

            if (_environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Super.Paula.Server V1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UsePaulaAppAuthentication();
            app.UsePaulaAppState();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPaulaServer();
                endpoints.MapGet("", () => "It works!");
            });
        }

        public async Task HandleError(HttpContext context)
        {
            var authorizeAttribute = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>();
            var connectionIpAddress = context.Connection.RemoteIpAddress?.ToString();

            if (authorizeAttribute == null && connectionIpAddress != null)
            {
                context.RequestServices
                    .GetRequiredService<IConnectionViolationManager>()
                    .Trace(connectionIpAddress);
            }

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new ProblemDetails(null)
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

            if (exception is ValidationException validationException)
            {
                var errors = validationException.Errors;
                var problemDetails = new ProblemDetails(errors)
                {
                    Detail = exception.StackTrace ?? string.Empty,
                    Title = exception.Message,
                    Status = statusCode,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problemDetails, null, "application/problem+json");
            }
            else
            {
                var errors = new Dictionary<string, FormattableString[]> 
                {
                    [string.Empty] = GetInnerExceptions(exception).ToArray()
                };
                
                var problemDetails = new ProblemDetails(errors)
                {
                    Detail = exception.StackTrace ?? string.Empty,
                    Title = exception.Message,
                    Status = statusCode,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problemDetails, null, "application/problem+json");
            }
        }

        public IEnumerable<FormattableString> GetInnerExceptions(Exception exception)
        {
            var innerException = exception;
            do
            {
                yield return $"{innerException.Message}";
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }
    }
}