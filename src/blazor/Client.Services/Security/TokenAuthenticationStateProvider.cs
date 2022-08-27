using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application.Authentication;
using Super.Paula.Client.Storage;
using Super.Paula.Shared.Security;

namespace Super.Paula.Client.Security;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorage _localStorage;
    private readonly IAuthenticationRequestHandler _authenticationRequestHandler;
    private Task<AuthenticationState>? _authenticationState;

    private static bool _verified = false;

    public TokenAuthenticationStateProvider(
        ILocalStorage localStorage, 
        IAuthenticationRequestHandler authenticationRequestHandler)
    {
        _localStorage = localStorage;
        _localStorage.Changed += OnChanged;

        _authenticationRequestHandler = authenticationRequestHandler;
    }

    private void OnChanged(object? sender, LocalStorageEventArgs e)
    {
        if (e.Key != "token" &&
            e.Key != "authorization-filter")
        {
            return;
        }

        _authenticationState = null;

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_verified)
        {
            await _authenticationRequestHandler.VerifyAsync();
            _verified = true;
        }

        _authenticationState ??= CreateAuthenticationStateAsync();

        return await _authenticationState;
    }

    private async Task<AuthenticationState> CreateAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<Token>("token");
        var authorizationFilter = (await _localStorage.GetItemAsync<string[]>("authorization-filter"))
            ?? Array.Empty<string>();

        if (token == null)
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        Enumerable.Empty<Claim>())));
        }

        var claims = token.ToClaims()
            .Concat(authorizationFilter.Select(x => new Claim("AuthorizationFilter", x)));

        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity(claims, "password")));
    }
}