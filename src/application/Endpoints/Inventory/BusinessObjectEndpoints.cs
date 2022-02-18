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

            endpoints.MapCommands(
                "/business-objects/{businessObject}",
                ("/assign-inspection", AssignInspection),
                ("/cancel-inspection", CancelInspection),
                ("/schedule-inspection-audit/{inspection}", ScheduleInspectionAudit),
                ("/drop-inspection-audit/{inspection}", DropInspectionAudit),
                ("/create-inspection-audit", CreateInspectionAudit),
                ("/change-inspection-audit/{inspection}", ChangeInspectionAudit),
                ("/annotate-inspection-audit/{inspection}", AnnotateInspectionAudit));

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
            (IBusinessObjectHandler handler, string businessObject)
                => handler.DeleteAsync(businessObject);

        private static Delegate AssignInspection =>
            [Authorize("ManagementFull")]
            (IBusinessObjectHandler handler, string businessObject, AssignInspectionRequest request)
                => handler.AssignInspectionAsync(businessObject, request);

        private static Delegate CancelInspection =>
            [Authorize("ManagementFull")]
            (IBusinessObjectHandler handler, string businessObject, CancelInspectionRequest request)
                => handler.CancelInspectionAsync(businessObject, request);

        private static Delegate ScheduleInspectionAudit =>
            [Authorize("ManagementFull")]
            (IBusinessObjectHandler handler, string businessObject, string inspection, ScheduleInspectionAuditRequest request)
                => handler.ScheduleInspectionAuditAsync(businessObject, inspection, request);

        private static Delegate DropInspectionAudit =>
            [Authorize("ManagementFull")]
            (IBusinessObjectHandler handler, string businessObject, string inspection, DropInspectionAuditRequest request)
            => handler.DropInspectionAuditAsync(businessObject, inspection, request);

        private static Delegate Search =>
            [Authorize("ManagementRead")]
            (IBusinessObjectHandler handler,
                [FromQuery(Name = "q")] string query)
                => handler.SearchAsync(query);

        private static Delegate ChangeInspectionAudit =>
            [Authorize("AuditingFull")]
            (IBusinessObjectHandler handler, string businessObject, string inspection, ChangeInspectionAuditRequest request)
                => handler.ChangeInspectionAuditAsync(businessObject, inspection, request);

        private static Delegate AnnotateInspectionAudit =>
            [Authorize("AuditingFull")]
            (IBusinessObjectHandler handler, string businessObject, string inspection, AnnotateInspectionAuditRequest request)
                => handler.AnnotateInspectionAuditAsync(businessObject, inspection, request);

        private static Delegate CreateInspectionAudit =>
            [Authorize("AuditingFull")]
            (IBusinessObjectHandler handler, string businessObject, CreateInspectionAuditRequest request)
                => handler.CreateInspectionAuditAsync(businessObject, request);
    }
}