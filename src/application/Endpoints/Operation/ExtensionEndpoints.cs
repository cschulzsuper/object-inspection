using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Operation.Requests;
using System;

namespace Super.Paula.Application.Operation
{
    public static class ExtensionEndpoints
    {
        public static IEndpointRouteBuilder MapExtension(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapRestCollection(
                "Extensions",
                "/extensions",
                "/{aggregateType}",
                Get,
                GetAll,
                Create,
                null,
                Delete);

            endpoints.MapRestCollection(
                "Extension Fields",
                "/extensions/{aggregateType}/fields",
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
            (IExtensionHandler handler, string aggregateType)
                => handler.GetAsync(aggregateType);

        private static Delegate GetAll =>
            [Authorize("ManagementRead")]
            (IExtensionHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, ExtensionRequest request)
                => handler.CreateAsync(request);

        private static Delegate Delete =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, string aggregateType, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteAsync(aggregateType, etag);

        private static Delegate CreateField =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, string aggregateType, ExtensionFieldRequest request)
                => handler.CreateFieldAsync(aggregateType, request);

        private static Delegate DeleteField =>
            [Authorize("ManagementFull")]
            (IExtensionHandler handler, string aggregateType, string field, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteFieldAsync(aggregateType, field, etag);
    }
}