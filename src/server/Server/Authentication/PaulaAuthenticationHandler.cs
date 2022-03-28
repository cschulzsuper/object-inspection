using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Operation;
using Super.Paula.Authorization;
using Super.Paula.Environment;
using System.Security.Claims;
using System.Threading.Tasks;
using IAuthenticationHandler = Microsoft.AspNetCore.Authentication.IAuthenticationHandler;

namespace Super.Paula.Authentication
{
    public class PaulaAuthenticationHandler : IAuthenticationHandler
    {
        private IConnectionManager? _connectionManager;
        private AuthenticationScheme? _scheme;
        private HttpContext? _context;
        private AppSettings? _appSettings;
        private IAuthorizationTokenHandler? _tokenAuthorizationFilter;

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _scheme = scheme;
            _context = context;
            _connectionManager = context.RequestServices.GetRequiredService<IConnectionManager>();
            _appSettings = context.RequestServices.GetRequiredService<AppSettings>();
            _tokenAuthorizationFilter = context.RequestServices.GetRequiredService<IAuthorizationTokenHandler>();

            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var authorizationHeader = _context?.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader))
                if (_context?.Request.Query.ContainsKey("access_token") == true)
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

            if (token == null)
            {
                return null;
            }

            if (token.Identity == null ||
                token.Proof == null)
            {
                return null;
            }

            var validIdentity = _connectionManager!.Verify(
                token.Identity,
                token.Proof);

            if (!validIdentity)
            {
                return null;
            }

            if (token.Organization != null &&
                token.Inspector != null)
            {
                var validInspector = _connectionManager!.Verify(
                    $"{token.Organization}:{token.Inspector}",
                    token.Proof);

                if (!validInspector &&
                    _appSettings!.MaintainerIdentity == token.Identity)
                {
                    validInspector = _connectionManager!.Verify(
                        $"{token.ImpersonatorOrganization}:{token.ImpersonatorInspector}",
                        token.Proof);
                }

                if (!validInspector)
                {
                    return null;
                }
            }

            _tokenAuthorizationFilter?.RewriteAuthorizations(token);

            return token;
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
