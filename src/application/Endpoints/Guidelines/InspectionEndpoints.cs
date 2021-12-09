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
            [Authorize("Inspector")] 
            (IInspectionHandler handler, string inspection)
                => handler.GetAsync(inspection);

        private static Delegate GetAll =>
            [Authorize("Inspector")] 
            (IInspectionHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("ChiefInspector")]
            (IInspectionHandler handler, InspectionRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("ChiefInspector")]
            (IInspectionHandler handler, string inspection, InspectionRequest request)
                => handler.ReplaceAsync(inspection, request);

        private static Delegate Delete =>
            [Authorize("ChiefInspector")]
            (IInspectionHandler handler, string inspection)
                => handler.DeleteAsync(inspection);

        private static Delegate Activate =>
             [Authorize("ChiefInspector")] 
            (IInspectionHandler handler, string inspection)
                => handler.ActivateAsync(inspection);

        private static Delegate Deactivate =>
            [Authorize("ChiefInspector")]
            (IInspectionHandler handler, string inspection)
                => handler.DeactivateAsync(inspection);
    }
}