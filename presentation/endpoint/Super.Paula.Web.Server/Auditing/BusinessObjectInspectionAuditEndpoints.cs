using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Auditing.Requests;

namespace Super.Paula.Auditing
{
    public static class BusinessObjectInspectionAuditEndpoints
    {
        public static IEndpointRouteBuilder MapBusinessObjectInspectionAudit(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/business-objects/{businessObject}/inspection-audits",
                "/business-objects/{businessObject}/inspection-audits/{inspection}/{date:int}/{time:int}",
                Get,
                GetAllForBusinessObject,
                Create,
                Replace,
                Delete);

            endpoints.MapQueries(
                "/inspection-audits",
                ("", GetAll),
                ("/search", Search));

            endpoints.MapQueries(
                "/business-objects",
                ("/{businessObject}/inspection-audits/search", SearchForBusinessObject));

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

        private static Delegate GetAllForBusinessObject =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject)
                => handler.GetAllForBusinessObject(businessObject);

        private static Delegate Create =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, BusinessObjectInspectionAuditRequest request)
                => handler.CreateAsync(businessObject, request);

        private static Delegate Replace =>
            [Authorize("Inspector")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request)
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