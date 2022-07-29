using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration
{
    public static class OrganizationEndpoints
    {
        public static IEndpointRouteBuilder MapOrganization(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapRestCollection(
                "Organizations",
                "/organizations",
                "/{organization}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapRestResouceCommands(
                "Organizations",
                "/organizations/{organization}",
                ("/activate", Activate),
                ("/deactivate", Deactivate));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, string organization)
                => handler.GetAsync(organization);

        private static Delegate GetAll =>
            //[Authorize("Maintainance")]
            (IOrganizationHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            //[Authorize("Maintainance")]
            [UseOrganizationFromRoute]
            (IOrganizationHandler handler, OrganizationRequest request)
                    => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, string organization, OrganizationRequest request)
                => handler.ReplaceAsync(organization, request);

        private static Delegate Delete =>
            [Authorize("Maintainance")]
            [UseOrganizationFromRoute]
            (IOrganizationHandler handler, string organization, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteAsync(organization, etag);

        private static Delegate Activate =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, string organization, [FromHeader(Name = "If-Match")] string etag)
                => handler.ActivateAsync(organization, etag);

        private static Delegate Deactivate =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, string organization, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeactivateAsync(organization, etag);
    }
}