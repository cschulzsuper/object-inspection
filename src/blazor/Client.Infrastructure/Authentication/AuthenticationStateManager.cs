using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Client.Storage;
using Super.Paula.Environment;

namespace Super.Paula.Client.Authentication
{
    public class AuthenticationStateManager : AuthenticationStateProvider
    {
        private readonly AppAuthentication _appAuthentication;
        private readonly ILocalStorage _localStorage;

        public AuthenticationStateManager(
            AppAuthentication appAuthentication,
            ILocalStorage localStorage)
        {
            _appAuthentication = appAuthentication;
            _localStorage = localStorage;
        }

        public async Task PersistAuthenticationStateAsync()
        {
            _appAuthentication.Ticks = DateTime.Now.Ticks;

            await _localStorage.SetItemAsync("app-authentication", _appAuthentication);
            NotifyAuthenticationStateChanged(CreatePrincipalAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var appAuthentication = await _localStorage.GetItemAsync<AppAuthentication>("app-authentication");

            if (_appAuthentication.Ticks < appAuthentication?.Ticks)
            {
                _appAuthentication.Ticks = appAuthentication.Ticks;
                _appAuthentication.Bearer = appAuthentication.Bearer;
                _appAuthentication.Inspector = appAuthentication.Inspector;
                _appAuthentication.Organization = appAuthentication.Organization;
                _appAuthentication.ImpersonatorBearer = appAuthentication.ImpersonatorBearer;
                _appAuthentication.ImpersonatorInspector = appAuthentication.ImpersonatorInspector;
                _appAuthentication.ImpersonatorOrganization = appAuthentication.ImpersonatorOrganization;
                _appAuthentication.Authorizations = appAuthentication.Authorizations;
                _appAuthentication.AuthorizationsFilter = appAuthentication.AuthorizationsFilter;
            }

            return await CreatePrincipalAuthenticationStateAsync();
        }

        private Task<AuthenticationState> CreatePrincipalAuthenticationStateAsync()
        {
            var identity = string.IsNullOrWhiteSpace(_appAuthentication.Bearer)
                ? new ClaimsIdentity(Enumerable.Empty<Claim>(), "Password")
                : new ClaimsIdentity(Enumerable.Empty<Claim>());

            return Task.FromResult(
                new AuthenticationState(
                    new ClaimsPrincipal(identity)));
        }
    }
}
