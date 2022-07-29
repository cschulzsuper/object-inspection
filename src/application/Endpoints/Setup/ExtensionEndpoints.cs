using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Setup.Requests;
using System;

namespace Super.Paula.Application.Setup
{
    public static class ExtensionEndpoints
    {
        public static IEndpointRouteBuilder MapExtension(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapRestCollection(
                "Extensions",
                "/extensions",
                "/{type}",
                Get,
                GetAll,
                Create,
                null,
                Delete);

            endpoints.MapRestCollection(
                "Extension Fields",
                "/extensions/{type}/fields",
                "/{field}",
                null,
                null,
                CreateField,
                null,
                DeleteField);

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("AuditingLimited")]
            (IExtensionHandler handler, string extension)
                => handler.GetAsync(extension);

        private static Delegate GetAll =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, ExtensionRequest request)
                => handler.CreateAsync(request);

        private static Delegate Delete =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, string type, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteAsync(type, etag);

        private static Delegate CreateField =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, string type, ExtensionFieldRequest request)
                => handler.CreateFieldAsync(type, request);

        private static Delegate DeleteField =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, string type, string field, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteFieldAsync(type, field, etag);
    }
}