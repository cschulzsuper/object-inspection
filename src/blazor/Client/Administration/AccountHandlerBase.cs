using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;

namespace Super.Paula.Client.Administration
{
    internal class AccountHandlerBase : IAccountHandler
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
            var responseMessage = await _httpClient.PostAsJsonAsync("account/account/change-secret", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync()
        {
            var responseMessage = await _httpClient.GetAsync("account/query-authorizations");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<QueryAuthorizationsResponse>())!;
        }

        public async ValueTask RepairChiefInspectorAsync(RepairChiefInspectorRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/repair-chief-inspector", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterIdentityAsync(RegisterIdentityRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/register", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/register-organization", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<AssessChiefInspectorDefectivenessResponse> AssessChiefInspectorDefectivenessAsync(AssessChiefInspectorDefectivenessRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/assess-chief-inspector-defectiveness", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<AssessChiefInspectorDefectivenessResponse>())!;
        }

        public async ValueTask<SignInInspectorResponse> SignInInspectorAsync(SignInInspectorRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/sign-in-inspector", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<SignInInspectorResponse>())!;
        }

        public async ValueTask SignOutInspectorAsync()
        {
            var responseMessage = await _httpClient.PostAsync("account/sign-out-inspector", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<StartImpersonationResponse> StartImpersonationAsync(StartImpersonationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/start-impersonation", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<StartImpersonationResponse>())!;
        }

        public ValueTask StopImpersonationAsync()
            => ValueTask.CompletedTask;

        public async ValueTask VerifyAsync()
        {
            var responseMessage = await _httpClient.PostAsync("account/verify", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}