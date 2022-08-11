using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;

namespace Super.Paula.Application.Operation
{
    public static class ExtensionTypeEndpoints
    {
        public static IEndpointRouteBuilder MapExtensionType(this IEndpointRouteBuilder endpoints)
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
            [Authorize("ManagementFull")]
            (IExtensionTypeHandler handler)
                => handler.GetAll();
    }
}