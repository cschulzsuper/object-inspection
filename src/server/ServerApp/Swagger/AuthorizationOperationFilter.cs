using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Swagger
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
#if true
            var hasAuthorization = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is AuthorizeAttribute);

            if (!hasAuthorization)
            {
                return;
            }
#else
#endif

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
}