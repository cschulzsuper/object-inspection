using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Authorization;
using Super.Paula.Client.Storage;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Client.Authentication
{
    public class AuthenticationStateManager : AuthenticationStateProvider
    {
        private readonly ILocalStorage _localStorage;

        private Task<AuthenticationState>? _authenticationState;

        public AuthenticationStateManager(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
            _localStorage.Changed += OnChanged;
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
            if (_authenticationState == null)
            {
                _authenticationState = CreateAuthenticationStateAsync();
            }

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
}
