using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Inventory.Requests;
using System;
using System.Threading;

namespace Super.Paula.Application.Inventory
{
    public static class BusinessObjectEndpoints
    {
        public static IEndpointRouteBuilder MapBusinessObject(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/business-objects",
                "/business-objects/{businessObject}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapQueries(
                "/business-objects",
                ("/search", Search));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("AuditingRead")]
            (IBusinessObjectHandler handler, string businessObject)
                => handler.GetAsync(businessObject);

        private static Delegate GetAll =>
            [Authorize("ManagementRead")]
            (IBusinessObjectHandler handler,
                [FromQuery(Name = "q")] string query,
                [FromQuery(Name = "s")] int? skip,
                [FromQuery(Name = "t")] int take,
                CancellationToken cancellationToken)
                => handler.GetAll(query, skip ?? 0, take, cancellationToken);

        private static Delegate Create =>
            [Authorize("ManagementFull")]
            (IBusinessObjectHandler handler, BusinessObjectRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("ManagementFull")]
            (IBusinessObjectHandler handler, string businessObject, BusinessObjectRequest request)
                => handler.ReplaceAsync(businessObject, request);

        private static Delegate Delete =>
            [Authorize("ManagementFull")]
            (IBusinessObjectHandler handler, string businessObject, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteAsync(businessObject, etag);

        private static Delegate Search =>
            [Authorize("ManagementRead")]
            (IBusinessObjectHandler handler,
                [FromQuery(Name = "q")] string query)
                => handler.SearchAsync(query);
    }
}