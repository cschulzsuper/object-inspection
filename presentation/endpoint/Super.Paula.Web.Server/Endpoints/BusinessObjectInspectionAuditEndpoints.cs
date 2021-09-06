using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using System;

namespace Super.Paula.Web.Server.Endpoints
{
    public static class BusinessObjectInspectionAuditEndpoints
    {
        public static IEndpointRouteBuilder MapBusinessObjectInspectionAudit(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/inspection-audits",
                "/inspection-audits/{businessObject}/{inspection}/{date:int}/{time:int}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapQueries(
                "/inspection-audits",
                ("/search", Search),
                ("/search-for-business-object/{businessObject}", SearchForBusinessObject));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("Inspector")] 
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time)
                => handler.GetAsync(businessObject, inspection, date, time);

        private static Delegate GetAll =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, InspectionAuditRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time, InspectionAuditRequest request)
                => handler.ReplaceAsync(businessObject, inspection, date, time, request);

        private static Delegate Delete =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time)
                => handler.DeleteAsync(businessObject, inspection, date, time);

        private static Delegate Search =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, [FromQuery(Name = "business-object") ]string? businessObject, string? inspector, string? inspection)
                => handler.Search(businessObject,inspector,inspection);

        private static Delegate SearchForBusinessObject =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string? inspector, string? inspection)
                => handler.SearchForBusinessObject(businessObject, inspector, inspection);

    }
}