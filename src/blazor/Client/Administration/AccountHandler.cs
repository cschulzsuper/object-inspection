using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Client.Authentication;
using Super.Paula.Environment;

namespace Super.Paula.Client.Administration
{
    internal class AccountHandler : IAccountHandler
    {
        private readonly IAccountHandler _accountHandler;

        private readonly AppAuthentication _appAuthentication;
        private readonly Lazy<AuthenticationStateManager> _authenticationStateManager;

        public AccountHandler(
            IAccountHandler accountHandler,
            AppAuthentication appAuthentication,
            Lazy<AuthenticationStateManager> authenticationStateManager)
        {
            _accountHandler = accountHandler;
            _appAuthentication = appAuthentication;
            
            _authenticationStateManager = authenticationStateManager;
        }

        public ValueTask ChangeSecretAsync(ChangeSecretRequest request)
            => _accountHandler.ChangeSecretAsync(request);

        public ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync()
        {
            var authorizations = _appAuthentication.Authorizations
                .Where(x =>
                        _appAuthentication.AuthorizationsFilter.Any() == false ||
                        _appAuthentication.AuthorizationsFilter.Contains(x))
                .ToHashSet();

            return ValueTask.FromResult(new QueryAuthorizationsResponse { Values = authorizations } );
        }

        public ValueTask RepairChiefInspectorAsync(RepairChiefInspectorRequest request)
            => _accountHandler.RepairChiefInspectorAsync(request);

        public ValueTask RegisterIdentityAsync(RegisterIdentityRequest request)
            => _accountHandler.RegisterIdentityAsync(request);

        public ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
            => _accountHandler.RegisterOrganizationAsync(request);

        public ValueTask<AssessChiefInspectorDefectivenessResponse> AssessChiefInspectorDefectivenessAsync(AssessChiefInspectorDefectivenessRequest request)
            => _accountHandler.AssessChiefInspectorDefectivenessAsync(request);

        public async ValueTask<SignInInspectorResponse> SignInInspectorAsync(SignInInspectorRequest request)
        {
            var response = await _accountHandler.SignInInspectorAsync(request);

            _appAuthentication.Bearer = response.Bearer;
            _appAuthentication.Inspector = request.UniqueName;
            _appAuthentication.Organization = request.Organization;

            _appAuthentication.ImpersonatorBearer = string.Empty;
            _appAuthentication.ImpersonatorInspector = string.Empty;
            _appAuthentication.ImpersonatorOrganization = string.Empty;

            await _authenticationStateManager.Value.PersistAuthenticationStateAsync(notify: false);

            _appAuthentication.Authorizations = (await _accountHandler.QueryAuthorizationsAsync()).Values.ToArray();
            _appAuthentication.AuthorizationsFilter = Array.Empty<string>();

            await _authenticationStateManager.Value.PersistAuthenticationStateAsync();

            return response;
        }

        public async ValueTask SignOutInspectorAsync()
        {
            await _accountHandler.SignOutInspectorAsync();

            _appAuthentication.Bearer = string.Empty;
            _appAuthentication.Inspector = string.Empty;
            _appAuthentication.Organization = string.Empty;

            _appAuthentication.ImpersonatorBearer = string.Empty;
            _appAuthentication.ImpersonatorInspector = string.Empty;
            _appAuthentication.ImpersonatorOrganization = string.Empty;

            _appAuthentication.Authorizations = Array.Empty<string>();
            _appAuthentication.AuthorizationsFilter = Array.Empty<string>();

            await _authenticationStateManager.Value.PersistAuthenticationStateAsync();
        }

        public async ValueTask<StartImpersonationResponse> StartImpersonationAsync(StartImpersonationRequest request)
        {
            var response = await _accountHandler.StartImpersonationAsync(request);

            _appAuthentication.ImpersonatorBearer = _appAuthentication.Bearer;
            _appAuthentication.ImpersonatorInspector = _appAuthentication.Inspector;
            _appAuthentication.ImpersonatorOrganization = _appAuthentication.Organization;

            _appAuthentication.Bearer = response.Bearer;
            _appAuthentication.Inspector = request.UniqueName;
            _appAuthentication.Organization = request.Organization;

            await _authenticationStateManager.Value.PersistAuthenticationStateAsync(notify: false);

            _appAuthentication.Authorizations = (await _accountHandler.QueryAuthorizationsAsync()).Values.ToArray();
            _appAuthentication.AuthorizationsFilter = Array.Empty<string>();

            await _authenticationStateManager.Value.PersistAuthenticationStateAsync();

            return response;
        }

        public async ValueTask StopImpersonationAsync()
        {
            if (!string.IsNullOrWhiteSpace(_appAuthentication.ImpersonatorBearer))
            {
                _appAuthentication.Bearer = _appAuthentication.ImpersonatorBearer!;
                _appAuthentication.Inspector = _appAuthentication.ImpersonatorInspector!;
                _appAuthentication.Organization = _appAuthentication.ImpersonatorOrganization!;

                _appAuthentication.ImpersonatorBearer = string.Empty;
                _appAuthentication.ImpersonatorInspector = string.Empty;
                _appAuthentication.ImpersonatorOrganization = string.Empty;

                await _authenticationStateManager.Value.PersistAuthenticationStateAsync(notify: false);

                _appAuthentication.Authorizations = (await _accountHandler.QueryAuthorizationsAsync()).Values.ToArray();
                _appAuthentication.AuthorizationsFilter = Array.Empty<string>();

                await _authenticationStateManager.Value.PersistAuthenticationStateAsync();
            }
        }

        public async ValueTask VerifyAsync()
        {
            try
            {
                await _accountHandler.VerifyAsync();
            }
            catch
            {
                _appAuthentication.Ticks = DateTime.Now.Ticks;
                _appAuthentication.Bearer = string.Empty;
                _appAuthentication.Inspector = string.Empty;
                _appAuthentication.Organization = string.Empty;
                _appAuthentication.ImpersonatorBearer = string.Empty;
                _appAuthentication.ImpersonatorInspector = string.Empty;
                _appAuthentication.ImpersonatorOrganization = string.Empty;
                _appAuthentication.Authorizations = Array.Empty<string>();
                _appAuthentication.AuthorizationsFilter = Array.Empty<string>();
            }
        }
    }
}