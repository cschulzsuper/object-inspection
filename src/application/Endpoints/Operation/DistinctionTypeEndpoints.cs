using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ChristianSchulz.ObjectInspection.Application.Operation.Requests;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public static class DistinctionTypeEndpoints
{
    public static IEndpointRouteBuilder MapDistinctionType(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Distinction Types",
            "/distinction-types",
            "/{uniqueName}",
            Get,
            GetAll,
            Create,
            null,
            Delete);

        endpoints.MapRestCollection(
            "Distinction Types",
            "/distinction-types/{uniqueName}/fields",
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
    (IDistinctionTypeRequestHandler requestHandler, string uniqueName)
            => requestHandler.GetAsync(uniqueName);

    private static Delegate GetAll =>
        [Authorize("OnlyChiefOrObserver")]
        (IDistinctionTypeRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate Create =>
        [Authorize("OnlyChief")]
        (IDistinctionTypeRequestHandler requestHandler, DistinctionTypeRequest request)
            => requestHandler.CreateAsync(request);

    private static Delegate Delete =>
        [Authorize("OnlyChief")]
        (IDistinctionTypeRequestHandler requestHandler, string uniqueName, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(uniqueName, etag);

    private static Delegate CreateField =>
        [Authorize("OnlyChief")]
        (IDistinctionTypeRequestHandler requestHandler, string uniqueName, DistinctionTypeFieldRequest request)
            => requestHandler.CreateFieldAsync(uniqueName, request);

    private static Delegate DeleteField =>
        [Authorize("OnlyChief")]
        (IDistinctionTypeRequestHandler requestHandler, string uniqueName, string field, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteFieldAsync(uniqueName, field, etag);
}