using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auditing.Requests;

namespace Super.Paula.Application.Auditing
{
    public static class BusinessObjectInspectionAuditEndpoints
    {
        public static IEndpointRouteBuilder MapBusinessObjectInspectionAudit(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/business-objects/{businessObject}/inspection-audits",
                "/business-objects/{businessObject}/inspection-audits/{inspection}/{date}/{time}",
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
            [Authorize("RequiresInspectability")] 
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time)
                => handler.GetAsync(businessObject, inspection, date, time);

        private static Delegate GetAll =>
            [Authorize("RequiresInspectability")]
            (IBusinessObjectInspectionAuditHandler handler)
                => handler.GetAll();

        private static Delegate GetAllForBusinessObject =>
            [Authorize("RequiresInspectability")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject)
                => handler.GetAllForBusinessObject(businessObject);

        private static Delegate Create =>
            [Authorize("RequiresInspectability")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, BusinessObjectInspectionAuditRequest request)
                => handler.CreateAsync(businessObject, request);

        private static Delegate Replace =>
            [Authorize("RequiresInspectability")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request)
                => handler.ReplaceAsync(businessObject, inspection, date, time, request);

        private static Delegate Delete =>
            [Authorize("RequiresInspectability")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time)
                => handler.DeleteAsync(businessObject, inspection, date, time);

        private static Delegate Search =>
            [Authorize("RequiresInspectability")]
            (IBusinessObjectInspectionAuditHandler handler, [FromQuery(Name = "business-object") ]string? businessObject, string? inspector, string? inspection)
                => handler.Search(businessObject,inspector,inspection);

        private static Delegate SearchForBusinessObject =>
            [Authorize("RequiresInspectability")]
            (IBusinessObjectInspectionAuditHandler handler, string businessObject, string? inspector, string? inspection)
                => handler.SearchForBusinessObject(businessObject, inspector, inspection);

    }
}