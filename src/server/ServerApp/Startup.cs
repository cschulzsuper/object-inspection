using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Server.Swagger;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using ChristianSchulz.ObjectInspection.Server.SwaggerGen;
using ChristianSchulz.ObjectInspection.Server.SwaggerUI;

namespace ChristianSchulz.ObjectInspection.Server;

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
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = new CustomJsonCamelCaseNamingPolicy();
            //options.SerializerOptions.Converters.Add(new ObjectJsonConverter());
        });
        
        services.AddServer(_environment, _configuration);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(setup =>
        {
            setup.ConfigureDefault();
        });

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
            {
                var client = _configuration["ObjectInspection:Client"];

                if (client != null)
                {
                    policy.WithOrigins(client);
                }

                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

        services.AddApplicationInsightsTelemetry();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (!_environment.IsDevelopment())
        {
            app.UseBlacklist();
        }

        app.UseResponseCompression();
        app.UseExceptionHandler(appBuilder => appBuilder.Run(HandleError));

        if (_environment.IsDevelopment())
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChristianSchulz.ObjectInspection.Server V1");
                options.UseAccessTokenRequestInterceptor();
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseRequestUser();
        app.UseAuthorization();

        app.UseEndpointState();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapServer();

            if (_environment.IsDevelopment())
            {
                endpoints.MapSwagger(setupAction: setup => { setup.ConfigurePreSerializeFilters(); });
            }

            endpoints.MapGet("", () => "It works!").WithTags("Health");
        });

        app.ConfigureEvents();
        app.ConfigureWorker();
        app.ConfigureContinuations();
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

        var problemDetails = new ProblemDetails
        {
            Detail = exception.StackTrace ?? string.Empty,
            Title = exception.Message,
            Status = statusCode,
            Instance = context.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["erros"] = validationException.Errors?
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Select(y => y.ToString())
                .ToArray());
        }

        if (exception is IFormattableException formattableException)
        {
            problemDetails.Extensions["titleFormat"] = formattableException.MessageFormat;
            problemDetails.Extensions["titleArguments"] = formattableException.MessageArguments;
        }

        await context.Response.WriteAsJsonAsync(problemDetails, (JsonSerializerOptions?)null, "application/problem+json");
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