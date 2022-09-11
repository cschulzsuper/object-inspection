using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Super.Paula.BadgeSecurity;

public class BadgeAuthenticationHandler : IAuthenticationHandler
{
    private AuthenticationScheme? _scheme;
    private HttpContext? _context;
    private IBadgeHandler? _handler;

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        _scheme = scheme;
        _context = context;
        _handler = context.RequestServices.GetRequiredService<IBadgeHandler>();

        return Task.CompletedTask;
    }

    public Task<AuthenticateResult> AuthenticateAsync()
    {
        var encodedBadge =
            GetBadgeFromHeaders() ??
            GetBadgeFromCookies() ??
            GetBadgeFromQuery();

        if (encodedBadge == null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
        }

        var user = _handler!.Authenticate(encodedBadge);
        if (user == null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
        }

        var ticket = new AuthenticationTicket(user, _scheme!.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private string? GetBadgeFromHeaders()
    {
        var authorizationHeader = _context!.Request.Headers.Authorization.ToString();

        return !string.IsNullOrWhiteSpace(authorizationHeader)
            ? authorizationHeader.Replace("Bearer ", string.Empty)
            : null;
    }

    private string? GetBadgeFromCookies()
    {
        var hasAuthorizationCookie = _context!.Request.Cookies
            .TryGetValue("access_token", out var authorizationCookie);

        return hasAuthorizationCookie && !string.IsNullOrWhiteSpace(authorizationCookie)
            ? authorizationCookie
            : null;
    }

    private string? GetBadgeFromQuery()
    {
        var hasAuthorizationQuery = _context!.Request.Query
            .TryGetValue("access_token", out var authorizationQuery);

        return hasAuthorizationQuery && !string.IsNullOrWhiteSpace(authorizationQuery)
            ? authorizationQuery.ToString()
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