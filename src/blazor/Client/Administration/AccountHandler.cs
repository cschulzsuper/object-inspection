using System.Collections.Generic;
using System.Threading.Tasks;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Client.Authentication;

namespace Super.Paula.Client.Administration
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IAccountHandler _accountHandler;

        private readonly AuthenticationStateManager _authenticationStateManager;

        public AccountHandler(
            IAccountHandler accountHandler,
            AuthenticationStateManager authenticationStateManager)
        {
            _accountHandler = accountHandler;        
            _authenticationStateManager = authenticationStateManager;
        }

        public ValueTask ChangeSecretAsync(ChangeSecretRequest request)
            => _accountHandler.ChangeSecretAsync(request);

        public IAsyncEnumerable<AccountAuthorizationResponse> GetAuthorizations()
            => _accountHandler.GetAuthorizations();

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