using System;
using System.Linq;
using System.Threading.Tasks;
using Super.Paula.Application;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Client.Authentication;
using Super.Paula.Environment;

namespace Super.Paula.Client.Administration
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IAccountHandler _accountHandler;

        private readonly AppAuthentication _appAuthentication;
        private readonly AuthenticationStateManager _authenticationStateManager;

        public AccountHandler(
            IAccountHandler accountHandler,
            AppAuthentication appAuthentication,
            AuthenticationStateManager authenticationStateManager)
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

        public async ValueTask<string> SignInInspectorAsync(SignInInspectorRequest request)
        {
            var response = await _accountHandler.SignInInspectorAsync(request);

            await _authenticationStateManager.PersistAuthenticationStateAsync(response);

            return response;
        }

        public async ValueTask SignOutInspectorAsync()
        {
            await _accountHandler.SignOutInspectorAsync();
            await _authenticationStateManager.PersistAuthenticationStateAsync(string.Empty);
        }

        public async ValueTask<string> StartImpersonationAsync(StartImpersonationRequest request)
        {
            var response = await _accountHandler.StartImpersonationAsync(request);

            await _authenticationStateManager.PersistAuthenticationStateAsync(response);

            return response;
        }

        public async ValueTask<string> StopImpersonationAsync()
        {
            var response = await _accountHandler.StopImpersonationAsync();

            await _authenticationStateManager.PersistAuthenticationStateAsync(response);

            return response;
        }

        public async ValueTask VerifyAsync()
            => await _accountHandler.VerifyAsync();        
    }
}