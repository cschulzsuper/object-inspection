using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Environment;
using System;

namespace Super.Paula.Application.Administration
{
    public static class OrganizationEndpoints
    {
        public static IEndpointRouteBuilder MapOrganization(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/organizations",
                "/organizations/{organization}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapCommands(
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
            [Authorize("Maintainance")]
            (IOrganizationHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, OrganizationRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("Maintainance")]
            [IgnoreCurrentOrganization]
            (IOrganizationHandler handler, string organization, OrganizationRequest request)
                => handler.ReplaceAsync(organization, request);

        private static Delegate Delete =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, string organization)
                => handler.DeleteAsync(organization);

        private static Delegate Activate =>
            [Authorize("Maintainance")]
            [IgnoreCurrentOrganization]
            (IOrganizationHandler handler, string organization)
                => handler.ActivateAsync(organization);

        private static Delegate Deactivate =>
            [Authorize("Maintainance")]
            [IgnoreCurrentOrganization]
            (IOrganizationHandler handler, string organization)
                => handler.DeactivateAsync(organization);
    }
}