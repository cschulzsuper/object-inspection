using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auditing.Requests;
using System;

namespace Super.Paula.Application.Auditing;

public static class BusinessObjectInspectorEndpoints
{
    public static IEndpointRouteBuilder MapBusinessObjectInspector(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Business Object Inspectors",
            "/business-objects/{businessObject}/inspectors",
            "/{inspector}",
            Get,
            GetAllForBusinessObject,
            Create,
            Replace,
            Delete);

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectorRequestHandler requestHandler, string businessObject, string inspector)
            => requestHandler.GetAsync(businessObject, inspector);

    private static Delegate GetAllForBusinessObject =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectorRequestHandler requestHandler,
            string businessObject)
            => requestHandler.GetAllForBusinessObject(businessObject);

    private static Delegate Create =>
        [Authorize("OnlyChief")]
        (IBusinessObjectInspectorRequestHandler requestHandler,
            string businessObject,
            BusinessObjectInspectorRequest request)

            => requestHandler.CreateAsync(businessObject, request);

    private static Delegate Replace =>
        [Authorize("OnlyChief")]
        (IBusinessObjectInspectorRequestHandler requestHandler,
            string businessObject,
            string inspector,
            BusinessObjectInspectorRequest request)

            => requestHandler.ReplaceAsync(businessObject, inspector, request);

    private static Delegate Delete =>
        [Authorize("OnlyChief")]
        (IBusinessObjectInspectorRequestHandler requestHandler,
            string businessObject,
            string inspector,
            [FromHeader(Name = "If-Match")] string etag)

            => requestHandler.DeleteAsync(businessObject, inspector, etag);
}