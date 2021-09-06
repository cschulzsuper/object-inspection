using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Environment.AspNetCore;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;

namespace Super.Paula.Web.Server.Endpoints
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccount(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCommands(
                "/account",
                ("/sign-in-inspector", SignInInspector),
                ("/sign-out-inspector", SignOutInspector),
                ("/register-inspector", RegisterInspector),
                ("/register-organization", RegisterOrganization),
                ("/change-secret", ChangeSecret),
                ("/start-impersonation", StartImpersonation),
                ("/repair-chief-inspector", RepairChiefInspector),
                ("/assess-chief-inspector-defectiveness", AssessChiefInspectorDefectiveness));

            endpoints.MapQueries(
                "/account",
                ("/query-authorizations", QueryAuthorizations));

            return endpoints;
        }
        private static Delegate SignInInspector =>
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, SignInInspectorRequest request)
                => handler.SignInInspectorAsync(request);

        private static Delegate SignOutInspector =>
            [Authorize("Inspector")]
            (IAccountHandler handler)
                => handler.SignOutInspectorAsync();

        private static Delegate RegisterInspector =>
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, RegisterInspectorRequest request)
                => handler.RegisterInspectorAsync(request);

        private static Delegate RegisterOrganization =>
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, RegisterOrganizationRequest request)
                => handler.RegisterOrganizationAsync(request);

        private static Delegate ChangeSecret =>
            [Authorize("Inspector")]
            (IAccountHandler handler, ChangeSecretRequest request)
            => handler.ChangeSecretAsync(request);

        private static Delegate QueryAuthorizations =>
            (IAccountHandler handler)
                => handler.QueryAuthorizationsAsync();

        private static Delegate StartImpersonation =>
            [Authorize("Maintainer")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, StartImpersonationRequest request)
                => handler.StartImpersonationAsync(request);

        private static Delegate RepairChiefInspector =>
            [Authorize("Maintainer")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, RepairChiefInspectorRequest request)
                => handler.RepairChiefInspectorAsync(request);

        private static Delegate AssessChiefInspectorDefectiveness =>
            [Authorize("Maintainer")]
            [IgnoreCurrentOrganization]
            (IAccountHandler handler, AssessChiefInspectorDefectivenessRequest request)
                => handler.AssessChiefInspectorDefectivenessAsync(request);
    }
}