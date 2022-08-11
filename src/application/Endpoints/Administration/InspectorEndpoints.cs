using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration
{
    public static class InspectorEndpoints
    {
        public static IEndpointRouteBuilder MapInspector(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapRestCollection(
                "Inspectors",
                "/inspectors",
                "/{inspector}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapRestResouceCommands(
                "Inspectors",
                "/inspectors/{inspector}",
                ("/activate", Activate),
                ("/deactivate", Deactivate));

            endpoints.MapRestCollectionQueries(
                "Organization Inspectors",
                "/organizations",
                ("/{organization}/inspectors", GetAllForOrganization));

            endpoints.MapRestCollectionQueries(
                "Current Inspector",
                "/inspectors",
                ("/me", GetCurrent));

            endpoints.MapRestCollectionQueries(
                "Identity Inspectors",
                "/identities",
                ("/{identity}/inspectors", GetAllForIdentity));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("InspectorRead")]
            (IInspectorRequestHandler handler, string inspector)
                => handler.GetAsync(inspector);

        private static Delegate GetCurrent =>
            [Authorize("AuditingLimited")]
            (IInspectorRequestHandler handler)
                => handler.GetCurrentAsync();

        private static Delegate GetAll =>
            [Authorize("ManagementRead")]
            (IInspectorRequestHandler handler)
                => handler.GetAll();

        private static Delegate GetAllForOrganization =>
            [Authorize("ManagementRead")]
            [UseOrganizationFromRoute]
            (IInspectorRequestHandler handler, string organization)
                => handler.GetAllForOrganization(organization);

        private static Delegate GetAllForIdentity =>
            [Authorize("IdentityRead")]
            (IInspectorRequestHandler handler, string identity)
                => handler.GetAllForIdentity(identity);

        private static Delegate Create =>
            [Authorize("ManagementFull")]
            (IInspectorRequestHandler handler, InspectorRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("ManagementFull")]
            (IInspectorRequestHandler handler, string inspector, InspectorRequest request)
                => handler.ReplaceAsync(inspector, request);

        private static Delegate Delete =>
            [Authorize("ManagementFull")]
            (IInspectorRequestHandler handler, string inspector, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteAsync(inspector, etag);

        private static Delegate Activate =>
            [Authorize("ManagementFull")]
            (IInspectorRequestHandler handler, string inspector, [FromHeader(Name = "If-Match")] string etag)
                => handler.ActivateAsync(inspector, etag);

        private static Delegate Deactivate =>
           [Authorize("ManagementFull")]
           (IInspectorRequestHandler handler, string inspector, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeactivateAsync(inspector, etag);
    }
}