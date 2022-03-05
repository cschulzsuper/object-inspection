using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auditing.Requests;
using System;
using System.Threading;

namespace Super.Paula.Application.Auditing
{
    public static class BusinessObjectInspectionAuditRecordEndpoints
    {
        public static IEndpointRouteBuilder MapBusinessObjectInspectionAuditRecord(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/business-objects/{businessObject}/inspection-audit-records",
                "/business-objects/{businessObject}/inspections/{inspection}/audit-records/{date}/{time}",
                Get,
                GetAllForBusinessObject,
                Create,
                Replace,
                Delete);

            endpoints.MapQueries(
                "/inspection-audit-records",
                ("", GetAll),
                ("/search", Search));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionAuditRecordHandler handler,
                string businessObject, 
                string inspection,
                int date,
                int time)

                => handler.GetAsync(businessObject, inspection, date, time);

        private static Delegate GetAll =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionAuditRecordHandler handler,
                [FromQuery(Name = "q")] string query,
                [FromQuery(Name = "s")] int? skip,
                [FromQuery(Name = "t")] int take,
                CancellationToken cancellationToken)

                => handler.GetAll(query, skip ?? 0, take, cancellationToken);

        private static Delegate GetAllForBusinessObject =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionAuditRecordHandler handler,
                string businessObject,
                [FromQuery(Name = "s")] int? skip,
                [FromQuery(Name = "t")] int take)

                => handler.GetAllForBusinessObject(businessObject, skip ?? 0, take);

        private static Delegate Create =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionAuditRecordHandler handler,
                string businessObject,
                BusinessObjectInspectionAuditRecordRequest request)

                => handler.CreateAsync(businessObject, request);

        private static Delegate Replace =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionAuditRecordHandler handler, 
                string businessObject, 
                string inspection,
                int date,
                int time,
                BusinessObjectInspectionAuditRecordRequest request)

                => handler.ReplaceAsync(businessObject, inspection, date, time, request);

        private static Delegate Delete =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionAuditRecordHandler handler, 
                string businessObject,
                string inspection, 
                int date, 
                int time, 
                [FromHeader(Name = "If-Match")] string etag)

                => handler.DeleteAsync(businessObject, inspection, date, time, etag);

        private static Delegate Search =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionAuditRecordHandler handler,
                [FromQuery(Name = "q")] string query)

                => handler.SearchAsync(query);
    }
}