using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Guidelines.Requests;
using System;

namespace Super.Paula.Application.Guidelines
{
    public static class InspectionEndpoints
    {
        public static IEndpointRouteBuilder MapInspection(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/inspections",
                "/inspections/{inspection}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapCommands(
                "/inspections/{inspection}",
                ("/activate", Activate),
                ("/deactivate", Deactivate));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("AuditingRead")]
            (IInspectionHandler handler, string inspection)
                => handler.GetAsync(inspection);

        private static Delegate GetAll =>
            [Authorize("AuditingRead")]
            (IInspectionHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("ManagementFull")]
            (IInspectionHandler handler, InspectionRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("ManagementFull")]
            (IInspectionHandler handler, string inspection, InspectionRequest request)
                => handler.ReplaceAsync(inspection, request);

        private static Delegate Delete =>
            [Authorize("ManagementFull")]
            (IInspectionHandler handler, string inspection, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteAsync(inspection, etag);

        private static Delegate Activate =>
             [Authorize("ManagementFull")]
            (IInspectionHandler handler, string inspection, [FromHeader(Name = "If-Match")] string etag)
                => handler.ActivateAsync(inspection, etag);

        private static Delegate Deactivate =>
            [Authorize("ManagementFull")]
            (IInspectionHandler handler, string inspection, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeactivateAsync(inspection, etag);
    }
}