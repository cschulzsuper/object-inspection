using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ChristianSchulz.ObjectInspection.Application.Operation.Requests;
using System;
using ChristianSchulz.ObjectInspection.Application.Guidelines;
using ChristianSchulz.ObjectInspection.Application.Reporting;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public static class ReportEndpoints
{
    public static IEndpointRouteBuilder MapReport(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollectionQueries(
        "Reports",
        "/reports",
            ("/business-object-inspection-audit-record-result-report", GetBusinessObjectInspectionAuditRecordResultReport));

        return endpoints;
    }

    private static Delegate GetBusinessObjectInspectionAuditRecordResultReport =>
        [Authorize("OnlyChiefOrObserver")]
        (IReportHandler reportHandler, 
            [FromQuery(Name = "q")] string query)
            => reportHandler.GetBusinessObjectInspectionAuditRecordResultReportAsync(query);
}