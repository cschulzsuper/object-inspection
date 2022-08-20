using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Server.Swagger;

public class AuthorizationOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorization = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is AuthorizeAttribute);

        if (!hasAuthorization)
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
    {
        new OpenApiSecurityRequirement
        {
            [ new OpenApiSecurityScheme {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer"
                }
            }
            ] = new List<string>()
        }
    };
    }
}