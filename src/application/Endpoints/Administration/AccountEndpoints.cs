using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Environment;
using System;

namespace Super.Paula.Application.Administration
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccount(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCommands(
                "/account",
                ("/sign-in-inspector", SignInInspector),
                ("/sign-out-inspector", SignOutInspector),
                ("/register", Register),
                ("/register-organization", RegisterOrganization),
                ("/change-secret", ChangeSecret),
                ("/start-impersonation", StartImpersonation),
                ("/stop-impersonation", StopImpersonation),
                ("/repair-chief-inspector", RepairChiefInspector),
                ("/assess-chief-inspector-defectiveness", AssessChiefInspectorDefectiveness),
                ("/verify", Verify));

            endpoints.MapQueries(
                "/account",
                ("/authorizations", GetAuthorizations));

            return endpoints;
        }
        private static Delegate SignInInspector =>
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, SignInInspectorRequest request)
                => handler.SignInInspectorAsync(request);

        private static Delegate SignOutInspector =>
            [Authorize("RequiresWeekInspectability")]
            (IAccountHandler handler)
                => handler.SignOutInspectorAsync();

        private static Delegate Register =>
            (IAccountHandler handler, RegisterIdentityRequest request)
                => handler.RegisterIdentityAsync(request);

        private static Delegate RegisterOrganization =>
            (IAccountHandler handler, RegisterOrganizationRequest request)
                => handler.RegisterOrganizationAsync(request);

        private static Delegate ChangeSecret =>
            [Authorize("RequiresInspectability")]
            (IAccountHandler handler, ChangeSecretRequest request)
            => handler.ChangeSecretAsync(request);

        private static Delegate GetAuthorizations =>
            (IAccountHandler handler)
                => handler.GetAuthorizations();

        private static Delegate StartImpersonation =>
            [Authorize("RequiresMaintainability")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, StartImpersonationRequest request)
                => handler.StartImpersonationAsync(request);

        private static Delegate StopImpersonation =>
            [Authorize("RequiresWeekInspectability")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler)
                => handler.StopImpersonationAsync();

        private static Delegate RepairChiefInspector =>
            [Authorize("RequiresMaintainability")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, RepairChiefInspectorRequest request)
                => handler.RepairChiefInspectorAsync(request);

        private static Delegate AssessChiefInspectorDefectiveness =>
            [Authorize("RequiresMaintainability")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, AssessChiefInspectorDefectivenessRequest request)
                => handler.AssessChiefInspectorDefectivenessAsync(request);

        private static Delegate Verify =>
            [Authorize("RequiresWeekInspectability")]
            (IAccountHandler handler)
                => handler.VerifyAsync();
    }
}