using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Data.Annotations;
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
                ("/sign-in-inspector", SignInInspector),
                ("/start-impersonation", StartImpersonation),
                ("/stop-impersonation", StopImpersonation));

            return endpoints;
        }
        private static Delegate SignInInspector =>
            [IgnoreCurrentOrganization]
            [Authorize]
            (IAccountHandler handler, SignInInspectorRequest request)
                    => handler.SignInInspectorAsync(request);

        private static Delegate RegisterOrganization =>
            [Authorize]
            (IAccountHandler handler, RegisterOrganizationRequest request)
                => handler.RegisterOrganizationAsync(request);

        private static Delegate StartImpersonation =>
            [Authorize("Maintainance")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, StartImpersonationRequest request)
                => handler.StartImpersonationAsync(request);

        private static Delegate StopImpersonation =>
            [Authorize("AuditingRead")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler)
                => handler.StopImpersonationAsync();
    }
}