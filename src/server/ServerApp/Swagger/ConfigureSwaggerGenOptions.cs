using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Super.Paula.Swagger
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header
            });

            options.OperationFilter<AuthorizationOperationFilter>();
            options.SwaggerDoc("v1", new() { Title = "Super.Paula.Endpoints", Version = "v1" });
        }
    }
}