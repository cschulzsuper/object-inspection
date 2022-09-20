using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerGen;

public class BadgeAuthorizationPreSerializePathItemFilter : IPreSerializePathItemFilter
{
    public void Apply(OpenApiPathItem pathItem, PreSerializePathItemFilterContext pathItemContext)
    {
        var user = pathItemContext.HttpContext.User;
        if (user.IsAuthenticatedIdentity())
        {
            return;
        }

        var unauthorizedOperations = pathItem.Operations
            .Where(operation => operation.Value.Security
                .Any(securityRequirements => securityRequirements
                    .All(securityRequirement => !IsAuthorized(securityRequirement.Value, user))));

        foreach(var unauthorizedOperation in unauthorizedOperations)
        {
            pathItem.Operations.Remove(unauthorizedOperation.Key);
        }
    }

    public static bool IsAuthorized(IList<string> validAuthorizations, ClaimsPrincipal user)
    {
        var validAuthorizationRequired = validAuthorizations.Any();

        if (!validAuthorizationRequired)
        {
            return true;
        }

        var hasValidAuthorization = user.Claims.HasAnyAuthorization(validAuthorizations);
        return hasValidAuthorization;
    }
}
