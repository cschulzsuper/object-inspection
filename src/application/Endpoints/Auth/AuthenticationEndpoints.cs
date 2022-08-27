using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auth.Requests;
using System;

namespace Super.Paula.Application.Auth;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthentication(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestResouceCommands(
            "Identities",
            "/identities",
            ("/register", Register));

        endpoints.MapRestResouceCommands(
            "Identities",
            "/identities/{identity}",
            ("/sign-in", SignIn),
            ("/reset", Reset));

        endpoints.MapRestResouceCommands(
            "Current Identity",
            "/identities/me",
            ("/sign-out", SignOut),
            ("/change-secret", ChangeSecret));

        return endpoints;
    }
    private static Delegate SignIn =>
        (IAuthenticationRequestHandler requestHandler, string identity, SignInIdentityRequest request)
            => requestHandler.SignInAsync(identity, request);

    private static Delegate SignOut =>
        [Authorize]
    (IAuthenticationRequestHandler requestHandler)
            => requestHandler.SignOutAsync();

    private static Delegate Register =>
        (IAuthenticationRequestHandler requestHandler, RegisterIdentityRequest request)
            => requestHandler.RegisterAsync(request);

    private static Delegate ChangeSecret =>
        [Authorize]
    (IAuthenticationRequestHandler requestHandler, ChangeIdentitySecretRequest request)
            => requestHandler.ChangeSecretAsync(request);

    private static Delegate Reset =>
        [Authorize("OnlyMaintainer")]
    (IAuthenticationRequestHandler requestHandler, string identity, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.ResetAsync(identity, etag);
}