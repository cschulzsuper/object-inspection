using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Environment;
using System;

namespace Super.Paula.Application.Administration
{
    public static class AuthenticationEndpoints
    {
        public static IEndpointRouteBuilder MapAuthentication(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCommands(
                "/authentication",
                ("/sign-in", SignIn),
                ("/sign-out", SignOut),
                ("/register", Register),
                ("/change-secret", ChangeSecret));

            return endpoints;
        }
        private static Delegate SignIn =>
            (IAuthenticationHandler handler, SignInRequest request)
                => handler.SignInAsync(request);

        private static Delegate SignOut =>
            [Authorize]
            (IAuthenticationHandler handler)
                => handler.SignOutAsync();

        private static Delegate Register =>
            (IAuthenticationHandler handler, RegisterRequest request)
                => handler.RegisterAsync(request);

        private static Delegate ChangeSecret =>
            [Authorize]
            (IAuthenticationHandler handler, ChangeSecretRequest request)
                => handler.ChangeSecretAsync(request);
    }
}