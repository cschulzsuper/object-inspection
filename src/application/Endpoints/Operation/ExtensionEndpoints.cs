using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ChristianSchulz.ObjectInspection.Application.Operation.Requests;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

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
            "Extensions",
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
        [Authorize("OnlyInspectorOrObserver")]
    (IExtensionRequestHandler requestHandler, string aggregateType)
            => requestHandler.GetAsync(aggregateType);

    private static Delegate GetAll =>
        [Authorize("OnlyChiefOrObserver")]
    (IExtensionRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate Create =>
        [Authorize("OnlyChief")]
    (IExtensionRequestHandler requestHandler, ExtensionRequest request)
            => requestHandler.CreateAsync(request);

    private static Delegate Delete =>
        [Authorize("OnlyChief")]
    (IExtensionRequestHandler requestHandler, string aggregateType, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(aggregateType, etag);

    private static Delegate CreateField =>
        [Authorize("OnlyChief")]
    (IExtensionRequestHandler requestHandler, string aggregateType, ExtensionFieldRequest request)
            => requestHandler.CreateFieldAsync(aggregateType, request);

    private static Delegate DeleteField =>
        [Authorize("OnlyChief")]
    (IExtensionRequestHandler requestHandler, string aggregateType, string field, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteFieldAsync(aggregateType, field, etag);
}