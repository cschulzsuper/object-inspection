using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Environment.AspNetCore;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;

namespace Super.Paula.Web.Server.Endpoints
{
    public static class InspectorEndpoints
    {
        public static IEndpointRouteBuilder MapInspector(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/inspectors",
                "/inspectors/{inspector}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapCommands(
                "/inspectors/{inspector}",
                ("/activate", Activate),
                ("/deactivate", Deactivate));

            endpoints.MapQueries(
                "/inspectors",
                ("/for-organization/{organization}", GetAllForOrganization));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("ChiefInspector")]
            (IInspectorHandler handler, string inspector)
                => handler.GetAsync(inspector);

        private static Delegate GetAll =>
            [Authorize("ChiefInspector")]
            (IInspectorHandler handler)
                => handler.GetAll();

        private static Delegate GetAllForOrganization =>
            [Authorize("Maintainer")]
            [IgnoreCurrentOrganization]
            (IInspectorHandler handler, string organization)
                => handler.GetAllForOrganization(organization);

        private static Delegate Create =>
            [Authorize("ChiefInspector")]
            (IInspectorHandler handler, InspectorRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("ChiefInspector")]
            (IInspectorHandler handler, string inspector, InspectorRequest request)
                => handler.ReplaceAsync(inspector, request);

        private static Delegate Delete =>
            [Authorize("ChiefInspector")]
            (IInspectorHandler handler, string inspector)
                => handler.DeleteAsync(inspector);

        private static Delegate Activate =>
            [Authorize("ChiefInspector")]
            (IInspectorHandler handler, string inspector)
                => handler.ActivateAsync(inspector);

        private static Delegate Deactivate =>
           [Authorize("ChiefInspector")]
            (IInspectorHandler handler, string inspector)
                => handler.DeactivateAsync(inspector);
    }
}