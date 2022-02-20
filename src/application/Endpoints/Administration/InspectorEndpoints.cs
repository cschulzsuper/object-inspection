using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration
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
                "/organizations",
                ("/{organization}/inspectors", GetAllForOrganization));

            endpoints.MapQueries(
                "/inspectors",
                ("/me", GetCurrent));

            endpoints.MapQueries(
                "/identities",
                ("/{identity}/inspectors", GetAllForIdentity));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("InspectorRead")]
            (IInspectorHandler handler, string inspector)
                => handler.GetAsync(inspector);

        private static Delegate GetCurrent =>
            [Authorize("AuditingRead")]
            (IInspectorHandler handler)
                => handler.GetCurrentAsync();

        private static Delegate GetAll =>
            [Authorize("ManagementRead")]
            (IInspectorHandler handler)
                => handler.GetAll();

        private static Delegate GetAllForOrganization =>
            [Authorize("ManagementRead")]
            [UseOrganizationFromRoute]
            (IInspectorHandler handler, string organization)
                => handler.GetAllForOrganization(organization);

        private static Delegate GetAllForIdentity =>
            [Authorize("IdentityRead")]
            (IInspectorHandler handler, string identity)
                => handler.GetAllForIdentity(identity);

        private static Delegate Create =>
            [Authorize("ManagementFull")]
            (IInspectorHandler handler, InspectorRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("ManagementFull")]
            (IInspectorHandler handler, string inspector, InspectorRequest request)
                => handler.ReplaceAsync(inspector, request);

        private static Delegate Delete =>
            [Authorize("ManagementFull")]
            (IInspectorHandler handler, string inspector)
                => handler.DeleteAsync(inspector);

        private static Delegate Activate =>
            [Authorize("ManagementFull")]
            (IInspectorHandler handler, string inspector)
                => handler.ActivateAsync(inspector);

        private static Delegate Deactivate =>
           [Authorize("ManagementFull")]
           (IInspectorHandler handler, string inspector)
                => handler.DeactivateAsync(inspector);
    }
}