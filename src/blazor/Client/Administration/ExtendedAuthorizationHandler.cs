using Super.Paula.Application.Administration;
using Super.Paula.Authorization;
using Super.Paula.Client.Storage;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class ExtendedAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IAuthorizationHandler _authorizationHandler;
        private readonly ILocalStorage _localStorage;

        public ExtendedAuthorizationHandler(
            IAuthorizationHandler authorizationHandler,
            ILocalStorage localStorage)
        {
            _authorizationHandler = authorizationHandler;
            _localStorage = localStorage;
        }

        public async ValueTask<string> AuthorizeAsync(string organization, string inspector)
        {
            var response = await _authorizationHandler.AuthorizeAsync(organization, inspector);

            var token = response.ToToken();
            await _localStorage.SetItemAsync("token", token);

            return response;
        }

        public async ValueTask<string> StartImpersonationAsync(string organization, string inspector)
        {
            var response = await _authorizationHandler.StartImpersonationAsync(organization, inspector);

            var token = response.ToToken();
            await _localStorage.SetItemAsync("token", token);

            return response;
        }

        public async ValueTask<string> StopImpersonationAsync()
        {
            var response = await _authorizationHandler.StopImpersonationAsync();

            var token = response.ToToken();
            await _localStorage.SetItemAsync("token", token);

            return response;
        }
    }
}