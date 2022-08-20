using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Inventory.Requests;
using System;
using System.Threading;

namespace Super.Paula.Application.Inventory;

public static class BusinessObjectEndpoints
{
    public static IEndpointRouteBuilder MapBusinessObject(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Business Objects",
            "/business-objects",
            "/{businessObject}",
            Get,
            GetAll,
            Create,
            Replace,
            Delete);

        endpoints.MapRestCollectionQueries(
            "Business Objects",
            "/business-objects",
            ("/search", Search));

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("AuditingLimited")]
    (IBusinessObjectRequestHandler requestHandler, string businessObject)
            => requestHandler.GetAsync(businessObject);

    private static Delegate GetAll =>
        [Authorize("ManagementRead")]
    (IBusinessObjectRequestHandler requestHandler,
            [FromQuery(Name = "q")] string query,
            [FromQuery(Name = "s")] int? skip,
            [FromQuery(Name = "t")] int take,
            CancellationToken cancellationToken)
            => requestHandler.GetAll(query, skip ?? 0, take, cancellationToken);

    private static Delegate Create =>
        [Authorize("ManagementFull")]
    (IBusinessObjectRequestHandler requestHandler, BusinessObjectRequest request)
            => requestHandler.CreateAsync(request);

    private static Delegate Replace =>
        [Authorize("ManagementFull")]
    (IBusinessObjectRequestHandler requestHandler, string businessObject, BusinessObjectRequest request)
            => requestHandler.ReplaceAsync(businessObject, request);

    private static Delegate Delete =>
        [Authorize("ManagementFull")]
    (IBusinessObjectRequestHandler requestHandler, string businessObject, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(businessObject, etag);

    private static Delegate Search =>
        [Authorize("ManagementRead")]
    (IBusinessObjectRequestHandler requestHandler,
            [FromQuery(Name = "q")] string query)
            => requestHandler.SearchAsync(query);
}