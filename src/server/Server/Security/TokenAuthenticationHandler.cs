﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Operation;
using Super.Paula.Shared.Environment;
using Super.Paula.Shared.Security;

namespace Super.Paula.Server.Security;

public class TokenAuthenticationHandler : IAuthenticationHandler
{
    private IConnectionManager? _connectionManager;
    private AuthenticationScheme? _scheme;
    private HttpContext? _context;
    private AppSettings? _appSettings;
    private IAuthorizationTokenHandler? _tokenAuthorizationHandler;

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        _scheme = scheme;
        _context = context;
        _connectionManager = context.RequestServices.GetRequiredService<IConnectionManager>();
        _appSettings = context.RequestServices.GetRequiredService<AppSettings>();
        _tokenAuthorizationHandler = context.RequestServices.GetRequiredService<IAuthorizationTokenHandler>();

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
        var hasAuthorizationCookie = _context!.Request.Cookies
            .TryGetValue("access_token", out var authorizationCookie);

        return hasAuthorizationCookie == true && !string.IsNullOrWhiteSpace(authorizationCookie)
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

        if (token.Identity == null || token.Proof == null)
        {
            return null;
        }

        var connectionAccount = token.Identity;
        var connectionProof = token.Proof;
        var connectionProofType = ConnectionProofTypes.Authentication;

        var validIdentity = _connectionManager!.Verify(connectionAccount, connectionProof, connectionProofType);

        if (!validIdentity)
        {
            if (token.Organization != null &&
                token.Inspector != null)
            {

                connectionAccount = $"{token.Organization}:{token.Inspector}";
                connectionProof = token.Proof;
                connectionProofType = ConnectionProofTypes.Authorization;

                var validInspector = _connectionManager!.Verify(connectionAccount, connectionProof, connectionProofType);

                if (!validInspector &&
                    _appSettings!.MaintainerIdentity == token.Identity)
                {
                    connectionAccount = $"{token.ImpersonatorOrganization}:{token.ImpersonatorInspector}";

                    validInspector = _connectionManager!.Verify(connectionAccount, connectionProof, connectionProofType);
                }

                if (!validInspector)
                {
                    return null;
                }
            }
        }

        _tokenAuthorizationHandler?.RewriteAuthorizations(token);

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