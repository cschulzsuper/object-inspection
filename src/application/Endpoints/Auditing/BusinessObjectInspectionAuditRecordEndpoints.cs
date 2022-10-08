using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ChristianSchulz.ObjectInspection.Application.Auditing.Requests;
using System;
using System.Threading;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public static class BusinessObjectInspectionAuditRecordEndpoints
{
    public static IEndpointRouteBuilder MapBusinessObjectInspectionAuditRecord(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Business Object Inspection Audit Records",
            "/business-objects/{businessObject}/inspection-audit-records",
            string.Empty,
            null,
            GetAllForBusinessObject,
            Create,
            null,
            null);

        endpoints.MapRestResource(
            "Business Object Inspection Audit Records",
            "/business-objects/{businessObject}/inspections/{inspection}/audit-records/{date}/{time}",
            Get,
            Replace,
            Delete);

        endpoints.MapRestCollectionQueries(
            "Inspection Audit Records",
            "/inspection-audit-records",
            ("", GetAll),
            ("/search", Search));

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("OnlyInspectorOrObserver")]
    (IBusinessObjectInspectionAuditRecordRequestHandler requestHandler,
            string businessObject,
            string inspection,
            int date,
            int time)

            => requestHandler.GetAsync(businessObject, inspection, date, time);

    private static Delegate GetAll =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectionAuditRecordRequestHandler requestHandler,
                [FromQuery(Name = "q")] string query,
                [FromQuery(Name = "s")] int? skip,
                [FromQuery(Name = "t")] int take,
                CancellationToken cancellationToken)

            => requestHandler.GetAll(query, skip ?? 0, take, cancellationToken);

    private static Delegate GetAllForBusinessObject =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectionAuditRecordRequestHandler requestHandler,
            string businessObject,
            [FromQuery(Name = "s")] int? skip,
            [FromQuery(Name = "t")] int take)

            => requestHandler.GetAllForBusinessObject(businessObject, skip ?? 0, take);

    private static Delegate Create =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectionAuditRecordRequestHandler requestHandler,
            string businessObject,
            BusinessObjectInspectionAuditRecordRequest request)

            => requestHandler.CreateAsync(businessObject, request);

    private static Delegate Replace =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectionAuditRecordRequestHandler requestHandler,
            string businessObject,
            string inspection,
            int date,
            int time,
            BusinessObjectInspectionAuditRecordRequest request)

            => requestHandler.ReplaceAsync(businessObject, inspection, date, time, request);

    private static Delegate Delete =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectionAuditRecordRequestHandler requestHandler,
            string businessObject,
            string inspection,
            int date,
            int time,
            [FromHeader(Name = "If-Match")] string etag)

            => requestHandler.DeleteAsync(businessObject, inspection, date, time, etag);

    private static Delegate Search =>
        [Authorize("OnlyInspectorOrObserver")]
        (IBusinessObjectInspectionAuditRecordRequestHandler requestHandler,
            [FromQuery(Name = "q")] string query)

            => requestHandler.SearchAsync(query);
}