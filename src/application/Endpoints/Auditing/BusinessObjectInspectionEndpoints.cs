using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auditing.Requests;
using System;

namespace Super.Paula.Application.Auditing
{
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
            (IBusinessObjectInspectionHandler handler, string businessObject, string inspection)
                => handler.GetAsync(businessObject, inspection);

        private static Delegate GetAllForBusinessObject =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject)
                => handler.GetAllForBusinessObject(businessObject);

        private static Delegate Create =>
            [Authorize("ManagementFull")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                BusinessObjectInspectionRequest request)

                => handler.CreateAsync(businessObject, request);

        private static Delegate Replace =>
            [Authorize("ManagementFull")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                string inspection,
                BusinessObjectInspectionRequest request)

                => handler.ReplaceAsync(businessObject, inspection, request);

        private static Delegate Delete =>
            [Authorize("ManagementFull")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                string inspection,
                [FromHeader(Name = "If-Match")] string etag)

                => handler.DeleteAsync(businessObject, inspection, etag);

        private static Delegate ReplaceAuditSchedule =>
            [Authorize("ManagementFull")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                string inspection,
                BusinessObjectInspectionAuditScheduleRequest request)

                => handler.ReplaceAuditScheduleAsync(businessObject, inspection, request);

        private static Delegate CreateAuditOmission =>
            [Authorize("ManagementFull")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                string inspection,
                BusinessObjectInspectionAuditOmissionRequest request)

                => handler.CreateAuditOmissionAsync(businessObject, inspection, request);

        private static Delegate CreateAudit =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                string inspection,
                BusinessObjectInspectionAuditRequest request)

                => handler.CreateAuditAsync(businessObject, inspection, request);

        private static Delegate ReplaceAudit =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                string inspection,
                BusinessObjectInspectionAuditRequest request)

                => handler.ReplaceAuditAsync(businessObject, inspection, request);

        private static Delegate ReplaceAuditAnnotation =>
            [Authorize("AuditingLimited")]
            (IBusinessObjectInspectionHandler handler,
                string businessObject,
                string inspection,
                BusinessObjectInspectionAuditAnnotationRequest request)

                => handler.ReplaceAuditAnnotationAsync(businessObject, inspection, request);
    }
}