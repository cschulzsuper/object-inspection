using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application;
using Super.Paula.Application.Administration;
using Super.Paula.Client.Storage;
using Super.Paula.Environment;

namespace Super.Paula.Client.Authentication
{
    public class AuthenticationStateManager : AuthenticationStateProvider
    {
        private readonly AppAuthentication _appAuthentication;
        private readonly ILocalStorage _localStorage;
        private readonly Lazy<IAccountHandler> _accountHandler;

        public AuthenticationStateManager(
            AppAuthentication appAuthentication,
            ILocalStorage localStorage,
            Lazy<IAccountHandler> accountHandler)
        {
            _appAuthentication = appAuthentication;
            _localStorage = localStorage;
            _accountHandler = accountHandler;
        }

        public async Task PersistAuthenticationStateAsync(bool notify = true)
        {
            var token = _appAuthentication.Token.ToToken();

            _appAuthentication.Ticks = DateTime.Now.Ticks;
            _appAuthentication.Organization = token?.Organization ?? string.Empty;
            _appAuthentication.Inspector = token?.Inspector ?? string.Empty;

            await _localStorage.SetItemAsync("app-authentication", _appAuthentication);
            if (notify)
            {
                NotifyAuthenticationStateChanged(CreateAuthenticationStateAsync());
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var appAuthentication = await _localStorage.GetItemAsync<AppAuthentication>("app-authentication");

            if (_appAuthentication.Ticks < appAuthentication?.Ticks)
            {
                _appAuthentication.Ticks = appAuthentication.Ticks;
                
                _appAuthentication.Token = appAuthentication.Token;
                _appAuthentication.Organization = appAuthentication.Organization;
                _appAuthentication.Inspector = appAuthentication.Inspector;

                _appAuthentication.Authorizations = appAuthentication.Authorizations;
                _appAuthentication.AuthorizationsFilter = appAuthentication.AuthorizationsFilter;

                await _accountHandler.Value.VerifyAsync();
            }

            return await CreateAuthenticationStateAsync();
        }

        private Task<AuthenticationState> CreateAuthenticationStateAsync()
        {
            var identity = string.IsNullOrWhiteSpace(_appAuthentication.Token)
                ? new ClaimsIdentity(Enumerable.Empty<Claim>(), "Password")
                : new ClaimsIdentity(Enumerable.Empty<Claim>());

            return Task.FromResult(
                new AuthenticationState(
                    new ClaimsPrincipal(identity)));
        }
    }
}
