using Super.Paula.Application.Auth;
using Super.Paula.Application.Auth.Exceptions;
using Super.Paula.Application.Auth.Requests;
using Super.Paula.Application.Auth.Responses;
using Super.Paula.Authorization;
using Super.Paula.Client.Storage;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class StoredTokenAuthenticationHandler : IAuthenticationHandler
    {
        private readonly IAuthenticationHandler _authenticationHandler;
        private readonly ILocalStorage _localStorage;

        public StoredTokenAuthenticationHandler(
            IAuthenticationHandler authenticationHandler,
            ILocalStorage localStorage)
        {
            _authenticationHandler = authenticationHandler;
            _localStorage = localStorage;
        }

        public ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request)
            => _authenticationHandler.ChangeSecretAsync(request);

        public ValueTask RegisterAsync(RegisterIdentityRequest request)
            => _authenticationHandler.RegisterAsync(request);

        public ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag)
            => _authenticationHandler.ResetAsync(identity, etag);

        public async ValueTask<string> SignInAsync(string identity, SignInIdentityRequest request)
        {
            var response = await _authenticationHandler.SignInAsync(identity, request);

            var token = response.ToToken();
            await _localStorage.SetItemAsync("token", token);

            return response;
        }

        public async ValueTask SignOutAsync()
        {
            try
            {
                await _authenticationHandler.SignOutAsync();
            }
            catch (SignOutException exception)
            {
                throw new SignOutException($"Could not sign out gracefully.", exception);
            }
            finally
            {
                await _localStorage.RemoveItemAsync("token");
                await _localStorage.RemoveItemAsync("authorization-filter");
            }
        }
    }
}
