using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Super.Paula.Authentication
{
    public class PaulaAuthenticationStateManager : AuthenticationStateProvider
    {
        private ISet<string> _authorizationFilter = ImmutableHashSet.Create<string>();
        private string _authenticationBearer = string.Empty;


        public ISet<string> GetAuthorizationsFilter()
            => _authorizationFilter;

        public void SetAuthorizationsFilter(params string[] authorizations)
        {
            _authorizationFilter = ImmutableHashSet.CreateRange(authorizations);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public string GetAuthenticationBearer()
            => _authenticationBearer;

        public void SetAuthenticationBearer(string bearer)
        {
            _authenticationBearer = bearer;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() 
        {
            var identity = string.IsNullOrEmpty(_authenticationBearer)
                ? new ClaimsIdentity(Enumerable.Empty<Claim>(), "Password")
                : new ClaimsIdentity(Enumerable.Empty<Claim>());

            return Task.FromResult(
                new AuthenticationState(
                    new ClaimsPrincipal(identity)));
        }
    }
}
