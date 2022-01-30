using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;
using Super.Paula.Application.Guidelines.Requests;

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
            [Authorize("RequiresWeekInspectability")] 
            (IInspectionHandler handler, string inspection)
                => handler.GetAsync(inspection);

        private static Delegate GetAll =>
            [Authorize("RequiresWeekInspectability")] 
            (IInspectionHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("RequiresManageability")]
            (IInspectionHandler handler, InspectionRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("RequiresManageability")]
            (IInspectionHandler handler, string inspection, InspectionRequest request)
                => handler.ReplaceAsync(inspection, request);

        private static Delegate Delete =>
            [Authorize("RequiresManageability")]
            (IInspectionHandler handler, string inspection)
                => handler.DeleteAsync(inspection);

        private static Delegate Activate =>
             [Authorize("RequiresManageability")] 
            (IInspectionHandler handler, string inspection)
                => handler.ActivateAsync(inspection);

        private static Delegate Deactivate =>
            [Authorize("RequiresManageability")]
            (IInspectionHandler handler, string inspection)
                => handler.DeactivateAsync(inspection);
    }
}