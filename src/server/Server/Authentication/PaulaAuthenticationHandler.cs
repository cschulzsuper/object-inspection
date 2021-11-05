using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
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
                var bearer = authorizationHeader.Replace("Bearer ", string.Empty);

                var subject = GetSubject(bearer);
                if (subject == null)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
                }
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, subject)
                };

                var identity = new ClaimsIdentity(claims, _scheme!.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, _scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
        }

        private string? GetSubject(string bearer)
        {
            var subject = Convert.FromBase64String(bearer);
            var subjectValue = Encoding.UTF8.GetString(subject);
            var subjectValues = subjectValue.Split(':', 3);

            if (subjectValues.Length != 3)
            {
                return null;
            }

            var validInspector = _connectionManager!.Verify(
                subjectValues[0],
                subjectValues[1],
                subjectValues[2]);

            if (validInspector)
            {
                return $"{subjectValues[0]}:{subjectValues[1]}";
            }

            var fallbackSubject = Convert.FromBase64String(subjectValues[2]);
            var fallbackSubjectValue = Encoding.UTF8.GetString(fallbackSubject);
            var fallbackSubjectValues = fallbackSubjectValue.Split(':', 3);

            if (_appSettings!.Maintainer != fallbackSubjectValues[1] ||
                _appSettings!.MaintainerOrganization != fallbackSubjectValues[0])
            {
                return null;
            }

            var validMaintainer = _connectionManager!.Verify(
                fallbackSubjectValues[0],
                fallbackSubjectValues[1],
                fallbackSubjectValues[2]);

            return validMaintainer
                ? $"{subjectValues[0]}:{subjectValues[1]}"
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
