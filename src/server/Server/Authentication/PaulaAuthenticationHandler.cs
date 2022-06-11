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
            var encodedAuthorizationToken = 
                GetTokenFromHeaders() ??
                GetTokenFromCookies() ??
                GetTokenFromQuery();

            if (!string.IsNullOrWhiteSpace(encodedAuthorizationToken))
            {
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

        private string? GetTokenFromHeaders()
        {
            var authorizationHeader = _context!.Request.Headers.Authorization.ToString();

            return !string.IsNullOrWhiteSpace(authorizationHeader)
                ? authorizationHeader.Replace("Bearer ", string.Empty)
                : null;
        }

        private string? GetTokenFromCookies()
        {
            var hasAuthorizatioCookie = _context!.Request.Cookies
                .TryGetValue("access_token", out var authorizationCookie);

            return hasAuthorizatioCookie == true && !string.IsNullOrWhiteSpace(authorizationCookie)
                ? authorizationCookie
                : null;
        }

        private string? GetTokenFromQuery()
        {
            var hasAuthorizationQuery = _context!.Request.Query
                .TryGetValue("access_token", out var authorizationQuery);

            return hasAuthorizationQuery == true && !string.IsNullOrWhiteSpace(authorizationQuery)
                ? authorizationQuery.ToString()
                : null;
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
