using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration
{
    public static class AuthorizationEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorization(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCommands(
                "/organizations/{organization}/inspectors/{inspector}",
                ("/authorize", Authorize),
                ("/impersonate", Impersonate));

            endpoints.MapCommands(
                "/inspector/current",
                ("/unmask", Unmask));

            endpoints.MapCommands(
                "/organizations/{organization}",
                ("/initialize", Initialize));

            endpoints.MapCommands(
                "/organizations",
                ("/register", Register));

            return endpoints;
        }

        private static Delegate Register =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, RegisterOrganizationRequest request)
                => handler.RegisterAsync(request);

        private static Delegate Initialize =>
            [Authorize("Maintainance")]
            (IOrganizationHandler handler, string organization, InitializeOrganizationRequest request)
                => handler.InitializeAsync(organization, request);

        private static Delegate Authorize =>
            [Authorize]
            [UseOrganizationFromRoute]
            (IAuthorizationHandler handler, string organization, string inspector)
                    => handler.AuthorizeAsync(organization, inspector);

        private static Delegate Impersonate =>
            [Authorize("Maintainance")]
            [UseOrganizationFromRoute]
            (IAuthorizationHandler handler, string organization, string inspector)
                    => handler.StartImpersonationAsync(organization, inspector);

        private static Delegate Unmask =>
            [Authorize("Maintainance")]
            (IAuthorizationHandler handler)
                => handler.StopImpersonationAsync();
    }
}