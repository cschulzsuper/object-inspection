using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auditing.Requests;
using System;
using System.Threading;

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

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("AuditingRead")]
        (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time)
                => handler.GetAsync(businessObject, inspection, date, time);

        private static Delegate GetAll =>
            [Authorize("AuditingRead")]
        (IBusinessObjectInspectionAuditHandler handler,
                [FromQuery(Name = "q")] string query,
                [FromQuery(Name = "s")] int? skip,
                [FromQuery(Name = "t")] int take,
                CancellationToken cancellationToken)
                => handler.GetAll(query, skip ?? 0, take, cancellationToken);

        private static Delegate GetAllForBusinessObject =>
            [Authorize("AuditingRead")]
        (IBusinessObjectInspectionAuditHandler handler,
                string businessObject,
                [FromQuery(Name = "s")] int? skip,
                [FromQuery(Name = "t")] int take)
                => handler.GetAllForBusinessObject(businessObject, skip ?? 0, take);

        private static Delegate Create =>
            [Authorize("AuditingFull")]
        (IBusinessObjectInspectionAuditHandler handler, string businessObject, BusinessObjectInspectionAuditRequest request)
                => handler.CreateAsync(businessObject, request);

        private static Delegate Replace =>
            [Authorize("AuditingFull")]
        (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request)
                => handler.ReplaceAsync(businessObject, inspection, date, time, request);

        private static Delegate Delete =>
            [Authorize("AuditingFull")]
        (IBusinessObjectInspectionAuditHandler handler, string businessObject, string inspection, int date, int time)
                => handler.DeleteAsync(businessObject, inspection, date, time);

        private static Delegate Search =>
            [Authorize("AuditingRead")]
        (IBusinessObjectInspectionAuditHandler handler,
                [FromQuery(Name = "q")] string query)
                => handler.SearchAsync(query);
    }
}