using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auditing.Requests;
using System;

namespace Super.Paula.Application.Auditing;

public static class BusinessObjectInspectionEndpoints
{
    public static IEndpointRouteBuilder MapBusinessObjectInspection(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Business Object Inspections",
            "/business-objects/{businessObject}/inspections",
            "/{inspection}",
            Get,
            GetAllForBusinessObject,
            Create,
            Replace,
            Delete);

        endpoints.MapRestResouceCommands(
            "Business Object Inspections",
             "/business-objects/{businessObject}/inspections/{inspection}",
             ("/replace-audit-schedule", ReplaceAuditSchedule),
             ("/create-audit-schedule-omission", CreateAuditOmission),
             ("/create-audit", CreateAudit),
             ("/replace-audit", ReplaceAudit),
             ("/replace-audit-annotation", ReplaceAuditAnnotation));

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("AuditingLimited")]
        (IBusinessObjectInspectionRequestHandler requestHandler, string businessObject, string inspection)
            => requestHandler.GetAsync(businessObject, inspection);

    private static Delegate GetAllForBusinessObject =>
        [Authorize("AuditingLimited")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject)
            => requestHandler.GetAllForBusinessObject(businessObject);

    private static Delegate Create =>
        [Authorize("ManagementFull")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            BusinessObjectInspectionRequest request)

            => requestHandler.CreateAsync(businessObject, request);

    private static Delegate Replace =>
        [Authorize("ManagementFull")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            string inspection,
            BusinessObjectInspectionRequest request)

            => requestHandler.ReplaceAsync(businessObject, inspection, request);

    private static Delegate Delete =>
        [Authorize("ManagementFull")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            string inspection,
            [FromHeader(Name = "If-Match")] string etag)

            => requestHandler.DeleteAsync(businessObject, inspection, etag);

    private static Delegate ReplaceAuditSchedule =>
        [Authorize("ManagementFull")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            string inspection,
            BusinessObjectInspectionAuditScheduleRequest request)

            => requestHandler.ReplaceAuditScheduleAsync(businessObject, inspection, request);

    private static Delegate CreateAuditOmission =>
        [Authorize("ManagementFull")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            string inspection,
            BusinessObjectInspectionAuditOmissionRequest request)

            => requestHandler.CreateAuditOmissionAsync(businessObject, inspection, request);

    private static Delegate CreateAudit =>
        [Authorize("AuditingLimited")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            string inspection,
            BusinessObjectInspectionAuditRequest request)

            => requestHandler.CreateAuditAsync(businessObject, inspection, request);

    private static Delegate ReplaceAudit =>
        [Authorize("AuditingLimited")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            string inspection,
            BusinessObjectInspectionAuditRequest request)

            => requestHandler.ReplaceAuditAsync(businessObject, inspection, request);

    private static Delegate ReplaceAuditAnnotation =>
        [Authorize("AuditingLimited")]
        (IBusinessObjectInspectionRequestHandler requestHandler,
            string businessObject,
            string inspection,
            BusinessObjectInspectionAuditAnnotationRequest request)

            => requestHandler.ReplaceAuditAnnotationAsync(businessObject, inspection, request);
}