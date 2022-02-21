using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class AccountHandlerBase : IAccountHandler
    {
        private readonly HttpClient _httpClient;

        public AccountHandlerBase(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/change-secret", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/register-organization", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterChiefInspectorAsync(string organization, RegisterChiefInspectorRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"account/register-chief-inspector/{organization}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<string> SignInInspectorAsync(string organization, string inspector)
        {
            var responseMessage = await _httpClient.PostAsync($"account/sign-in-inspector/{organization}/{inspector}", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadAsStringAsync())!;
        }

        public async ValueTask<string> StartImpersonationAsync(string organization, string inspector)
        {
            var responseMessage = await _httpClient.PostAsync($"account/start-impersonation/{organization}/{inspector}", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadAsStringAsync())!;
        }

        public async ValueTask<string> StopImpersonationAsync()
        {
            var responseMessage = await _httpClient.PostAsync("account/stop-impersonation", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadAsStringAsync())!;
        }
    }
}