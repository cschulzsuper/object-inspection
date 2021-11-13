using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Client.Storage;

namespace Super.Paula.Client.Authentication
{
    public class AuthenticationStateManager : AuthenticationStateProvider
    {
        private ISet<string> _authorizationFilter = ImmutableHashSet.Create<string>();
        private string _authorizationBearer = string.Empty;

        public ISet<string> GetAuthorizationsFilter()
            => _authorizationFilter;

        public void SetAuthorizationsFilter(params string[] authorizations)
        {
            _authorizationFilter = ImmutableHashSet.CreateRange(authorizations);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public string GetAuthenticationBearer()
        {
            return _authorizationBearer;
        }

        public void SetAuthenticationBearer(string bearer)
        {
            if (bearer != _authorizationBearer)
            {
                _authorizationBearer = bearer;
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = string.IsNullOrWhiteSpace(_authorizationBearer)
                ? new ClaimsIdentity(Enumerable.Empty<Claim>(), "Password")
                : new ClaimsIdentity(Enumerable.Empty<Claim>());

            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }
    }
}
