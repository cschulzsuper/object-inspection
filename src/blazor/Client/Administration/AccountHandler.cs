using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Authorization;
using Super.Paula.Client.Storage;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IAccountHandler _accountHandler;
        private readonly ILocalStorage _localStorage;

        public AccountHandler(
            IAccountHandler accountHandler,
            ILocalStorage localStorage)
        {
            _accountHandler = accountHandler;
            _localStorage = localStorage;
        }

        public ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
            => _accountHandler.RegisterOrganizationAsync(request);

        public async ValueTask<string> SignInInspectorAsync(SignInInspectorRequest request)
        {
            var response = await _accountHandler.SignInInspectorAsync(request);

            var token = response.ToToken();
            await _localStorage.SetItemAsync("token", token);

            return response;
        }

        public async ValueTask<string> StartImpersonationAsync(StartImpersonationRequest request)
        {
            var response = await _accountHandler.StartImpersonationAsync(request);

            var token = response.ToToken();
            await _localStorage.SetItemAsync("token", token);

            return response;
        }

        public async ValueTask<string> StopImpersonationAsync()
        {
            var response = await _accountHandler.StopImpersonationAsync();

            var token = response.ToToken();
            await _localStorage.SetItemAsync("token", token);

            return response;
        }
    }
}