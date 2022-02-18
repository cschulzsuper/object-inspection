using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Exceptions;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Authorization;
using Super.Paula.Client.Storage;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IAuthenticationHandler _authenticationHandler;
        private readonly ILocalStorage _localStorage;

        public AuthenticationHandler(
            IAuthenticationHandler authenticationHandler,
            ILocalStorage localStorage)
        {
            _authenticationHandler = authenticationHandler;
            _localStorage = localStorage;
        }

        public ValueTask ChangeSecretAsync(ChangeSecretRequest request)
            => _authenticationHandler.ChangeSecretAsync(request);

        public ValueTask RegisterAsync(RegisterRequest request)
            => _authenticationHandler.RegisterAsync(request);

        public async ValueTask<string> SignInAsync(SignInRequest request)
        {
            var response = await _authenticationHandler.SignInAsync(request);

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
