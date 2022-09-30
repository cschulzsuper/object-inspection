using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public class TokenAuthorizationOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authorizeAttribute = (AuthorizeAttribute?) context.ApiDescription
            .ActionDescriptor
            .EndpointMetadata
            .SingleOrDefault(x => x is AuthorizeAttribute);

        if (authorizeAttribute == null)
        {
            return;
        }

        var tokenAuthorizations = GetTokenAuthorizations(authorizeAttribute.Policy);

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearer"
                        }
                    },
                    tokenAuthorizations
                }
            }
        };
    }

    private static string[] GetTokenAuthorizations(string? policy)
    {
        return policy switch
        {
            "OnlyChief" 
                => new[] { "Chief" },

            "OnlyChiefOrObserver" 
                => new[] { "Chief", "Observer" },

            "OnlyChiefOrObserverOrInspectorOwner" 
                => new[] { "Inspector" },

            "OnlyImpersonator"
                => new[] { "Impersonator" },

            "OnlyInspector" 
                => new[] { "Inspector" },

            "OnlyInspectorOrObserver" 
                => new[] { "Inspector", "Observer" },

            "OnlyMaintainer"
                => new[] { "Maintainer" },

            "OnlyMaintainerOrIdentityOwner"
                => Array.Empty<string>(),

            _ => Array.Empty<string>()
        };
    }
}