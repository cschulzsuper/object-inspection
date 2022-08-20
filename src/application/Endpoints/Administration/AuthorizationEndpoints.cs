using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration;

public static class AuthorizationEndpoints
{
    public static IEndpointRouteBuilder MapAuthorization(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestResouceCommands(
            "Organization Inspectors",
            "/organizations/{organization}/inspectors/{inspector}",
            ("/authorize", Authorize),
            ("/impersonate", Impersonate));

        endpoints.MapRestResouceCommands(
            "Current Inspector",
            "/inspector/me",
            ("/unmask", Unmask));

        endpoints.MapRestResouceCommands(
            "Organizations",
            "/organizations/{organization}",
            ("/initialize", Initialize));

        endpoints.MapRestResouceCommands(
            "Organizations",
            "/organizations",
            ("/register", Register));

        return endpoints;
    }

    private static Delegate Register =>
        [Authorize("Maintenance")]
    (IOrganizationRequestHandler requestHandler, RegisterOrganizationRequest request)
            => requestHandler.RegisterAsync(request);

    private static Delegate Initialize =>
        [Authorize("Maintenance")]
    (IOrganizationRequestHandler requestHandler, string organization, InitializeOrganizationRequest request)
            => requestHandler.InitializeAsync(organization, request);

    private static Delegate Authorize =>
        [Authorize]
    [UseOrganizationFromRoute]
    (IAuthorizationRequestHandler requestHandler, string organization, string inspector)
                => requestHandler.AuthorizeAsync(organization, inspector);

    private static Delegate Impersonate =>
        [Authorize("Maintenance")]
    [UseOrganizationFromRoute]
    (IAuthorizationRequestHandler requestHandler, string organization, string inspector)
                => requestHandler.StartImpersonationAsync(organization, inspector);

    private static Delegate Unmask =>
        [Authorize("Impersonation")]
    (IAuthorizationRequestHandler requestHandler)
            => requestHandler.StopImpersonationAsync();
}