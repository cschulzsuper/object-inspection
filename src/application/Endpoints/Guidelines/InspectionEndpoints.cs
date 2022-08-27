using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Guidelines.Requests;
using System;

namespace Super.Paula.Application.Guidelines;

public static class InspectionEndpoints
{
    public static IEndpointRouteBuilder MapInspection(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Inspections",
            "/inspections",
            "{inspection}",
            Get,
            GetAll,
            Create,
            Replace,
            Delete);

        endpoints.MapRestResouceCommands(
            "Inspections",
            "/inspections/{inspection}",
            ("/activate", Activate),
            ("/deactivate", Deactivate));

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("OnlyInspectorOrObserver")]
        (IInspectionRequestHandler requestHandler, string inspection)
            => requestHandler.GetAsync(inspection);

    private static Delegate GetAll =>
        [Authorize("OnlyInspectorOrObserver")]
        (IInspectionRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate Create =>
        [Authorize("OnlyChief")]
        (IInspectionRequestHandler requestHandler, InspectionRequest request)
            => requestHandler.CreateAsync(request);

    private static Delegate Replace =>
        [Authorize("OnlyChief")]
        (IInspectionRequestHandler requestHandler, string inspection, InspectionRequest request)
            => requestHandler.ReplaceAsync(inspection, request);

    private static Delegate Delete =>
        [Authorize("OnlyChief")]
        (IInspectionRequestHandler requestHandler, string inspection, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(inspection, etag);

    private static Delegate Activate =>
        [Authorize("OnlyChief")]
        (IInspectionRequestHandler requestHandler, string inspection, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.ActivateAsync(inspection, etag);

    private static Delegate Deactivate =>
        [Authorize("OnlyChief")]
        (IInspectionRequestHandler requestHandler, string inspection, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeactivateAsync(inspection, etag);
}