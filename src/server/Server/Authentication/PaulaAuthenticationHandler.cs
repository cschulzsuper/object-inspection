using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Runtime;
using Super.Paula.Environment;

namespace Super.Paula.Authentication
{
    public class PaulaAuthenticationHandler : IAuthenticationHandler
    {
        private IConnectionManager? _connectionManager;
        private AuthenticationScheme? _scheme;
        private HttpContext? _context;
        private AppSettings? _appSettings;

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _scheme = scheme;
            _context = context;
            _connectionManager = context.RequestServices.GetRequiredService<IConnectionManager>();
            _appSettings = context.RequestServices.GetRequiredService<AppSettings>();

            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var authorizationHeader = _context?.Request.Headers.Authorization.ToString();


            if (string.IsNullOrWhiteSpace(authorizationHeader) &&
                _context?.Request.Query.ContainsKey("access_token") == true)
            {
                authorizationHeader = $"Bearer {_context.Request.Query["access_token"]}";
            }

            if (authorizationHeader != null &&
                authorizationHeader.StartsWith("Bearer "))
            {
                var encodedAuthorizationToken = authorizationHeader.Replace("Bearer ", string.Empty);

                var token = DecodeToken(encodedAuthorizationToken);
                if (token == null)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
                }

                var claims = token.ToClaims();
                var claimsIdentity = new ClaimsIdentity(claims, _scheme!.Name);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var ticket = new AuthenticationTicket(claimsPrincipal, _scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
        }

        private Token? DecodeToken(string encodedAuthorizationToken)
        {
            var token = encodedAuthorizationToken.ToToken();

            if (token == null ||
                token.Organization == null ||
                token.Inspector == null ||
                token.Proof == null)
            {
                return null;
            }

            var validInspector = _connectionManager!.Verify(
                token.Organization,
                token.Inspector,
                token.Proof);

            if (validInspector)
            {
                return token;
            }

            if (_appSettings!.Maintainer != token.ImpersonatorInspector ||
                _appSettings!.MaintainerOrganization != token.ImpersonatorOrganization)
            {
                return null;
            }

            var validMaintainer = _connectionManager!.Verify(
                token.ImpersonatorOrganization,
                token.ImpersonatorInspector,
                token.Proof);

            return validMaintainer
                ? token
                : null;
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            _context!.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            _context!.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }
    }
}
