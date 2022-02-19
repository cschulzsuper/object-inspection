using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Exceptions;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class AuthenticationHandlerBase : IAuthenticationHandler
    {
        private readonly HttpClient _httpClient;

        public AuthenticationHandlerBase(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("authentication/change-secret", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterAsync(RegisterRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("authentication/register", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<string> SignInAsync(SignInRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("authentication/sign-in", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadAsStringAsync())!;
        }

        public async ValueTask SignOutAsync()
        {
            try
            {
                var responseMessage = await _httpClient.PostAsync("authentication/sign-out", null);

                responseMessage.RuleOutProblems();
                responseMessage.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException exception)
            {
                throw new SignOutException($"Could not sign out.", exception);
            }
        }
    }
}
