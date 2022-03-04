using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccount(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCommands(
                "/account",
                ("/register-organization", RegisterOrganization),
                ("/register-chief-inspector/{organization}", RegisterChiefInspector),
                ("/sign-in-inspector/{organization}/{inspector}", SignInInspector),
                ("/start-impersonation/{organization}/{inspector}", StartImpersonation),
                ("/stop-impersonation", StopImpersonation));

            return endpoints;
        }
        private static Delegate SignInInspector =>
            [Authorize]
            [UseOrganizationFromRoute]
            (IAccountHandler handler, string organization, string inspector)
                    => handler.SignInInspectorAsync(organization, inspector);

        private static Delegate RegisterOrganization =>
            [Authorize]
            (IAccountHandler handler, RegisterOrganizationRequest request)
                => handler.RegisterOrganizationAsync(request);

        private static Delegate RegisterChiefInspector =>
            [Authorize]
            [UseOrganizationFromRoute]
            (IAccountHandler handler, string organization, RegisterChiefInspectorRequest request)
                => handler.RegisterChiefInspectorAsync(organization, request);

        private static Delegate StartImpersonation =>
            [Authorize("Maintainance")]
            [UseOrganizationFromRoute]
            (IAccountHandler handler, string organization, string inspector)
                    => handler.StartImpersonationAsync(organization, inspector);

        private static Delegate StopImpersonation =>
            [Authorize("AuditingLimited")]
            (IAccountHandler handler)
                => handler.StopImpersonationAsync();
    }
}