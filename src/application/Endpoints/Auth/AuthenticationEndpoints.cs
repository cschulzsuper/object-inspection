using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auth.Requests;
using System;

namespace Super.Paula.Application.Auth
{
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
            (IAuthenticationHandler handler, string identity, SignInIdentityRequest request)
                => handler.SignInAsync(identity, request);

        private static Delegate SignOut =>
            [Authorize]
        (IAuthenticationHandler handler)
                => handler.SignOutAsync();

        private static Delegate Register =>
            (IAuthenticationHandler handler, RegisterIdentityRequest request)
                => handler.RegisterAsync(request);

        private static Delegate ChangeSecret =>
            [Authorize]
        (IAuthenticationHandler handler, ChangeIdentitySecretRequest request)
                => handler.ChangeSecretAsync(request);

        private static Delegate Reset =>
            [Authorize("Maintainance")]
        (IAuthenticationHandler handler, string identity, [FromHeader(Name = "If-Match")] string etag)
                => handler.ResetAsync(identity, etag);
    }
}