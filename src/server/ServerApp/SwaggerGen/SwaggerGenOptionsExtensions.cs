using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Super.Paula.Server.SwaggerGen;

public static class SwaggerGenOptionsExtensions
{

    public static SwaggerGenOptions ConfigureDefault(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
        {
            Scheme = "bearer",
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header
        });

        options.OperationFilter<BadgeAuthorizationOperationFilter>();

        options.SwaggerDoc("v1", new() { Title = "Super.Paula.Server", Version = "v1" });

        options.OrderActionsBy(x => x.RelativePath);

        return options;
    }
}