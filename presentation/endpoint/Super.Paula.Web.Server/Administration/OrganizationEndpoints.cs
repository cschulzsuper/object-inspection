using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Administration.Requests;

namespace Super.Paula.Administration
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
            [Authorize("Maintainer")]
            (IOrganizationHandler handler, string organization)
                => handler.GetAsync(organization);

        private static Delegate GetAll =>
            [Authorize("Maintainer")]
            (IOrganizationHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("Maintainer")]
            (IOrganizationHandler handler, OrganizationRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("Maintainer")]
            (IOrganizationHandler handler, string organization, OrganizationRequest request)
                => handler.ReplaceAsync(organization, request);

        private static Delegate Delete =>
            [Authorize("Maintainer")]
            (IOrganizationHandler handler, string organization)
                => handler.DeleteAsync(organization);

        private static Delegate Activate =>
            [Authorize("Maintainer")]
            (IOrganizationHandler handler, string organization)
                => handler.ActivateAsync(organization);

        private static Delegate Deactivate =>
            [Authorize("Maintainer")]
            (IOrganizationHandler handler, string organization)
                => handler.DeactivateAsync(organization);
    }
}