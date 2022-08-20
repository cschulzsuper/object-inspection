using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;

namespace Super.Paula.Application.Operation;

public static class ExtensionTypeEndpoints
{
    public static IEndpointRouteBuilder MapExtensionAggrgateType(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Extensions",
            "/extension-types",
            "/{extensionType}",
            null,
            GetAll,
            null,
            null,
            null);

        return endpoints;
    }

    private static Delegate GetAll =>
        [Authorize("ManagementRead")]
        (IExtensionAggregateTypeRequestHandler requestHandler)
            => requestHandler.GetAll();
}