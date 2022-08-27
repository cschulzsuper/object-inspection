using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;

namespace Super.Paula.Application.Operation;

public static class ExtensionFieldTypeEndpoints
{
    public static IEndpointRouteBuilder MapExtensionFieldType(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Extensions",
            "/extension-field-types",
            "/{extensionFieldType}",
            null,
            GetAll,
            null,
            null,
            null);

        return endpoints;
    }

    private static Delegate GetAll =>
        [Authorize("OnlyChiefOrObserver")]
        (IExtensionAggregateTypeRequestHandler requestHandler)
            => requestHandler.GetAll();
}