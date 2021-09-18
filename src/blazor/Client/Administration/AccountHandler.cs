using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Authentication;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;

namespace Super.Paula.Client.Administration
{
    internal class AccountHandler : IAccountHandler
    {
        private readonly AppState _appState;
        private readonly AccountHandlerCache _accountHandlerCache;
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly HttpClient _httpClient;

        public AccountHandler(
            HttpClient httpClient,
            AppState appState,
            AppSettings appSettings,
            AccountHandlerCache accountHandlerCache,
            PaulaAuthenticationStateManager paulaAuthenticationStateManager)
        {
            _appState = appState;
            
            _accountHandlerCache = accountHandlerCache;
            _paulaAuthenticationStateManager = paulaAuthenticationStateManager;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        private void SetBearerOnHttpClient()
        {
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();

            _httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrWhiteSpace(bearer)
                    ? new AuthenticationHeaderValue("Bearer", bearer)
                    : null;
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            SetBearerOnHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync("account/account/change-secret", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync()
        {
            SetBearerOnHttpClient();

            if (_accountHandlerCache.QueryAuthorizationsResponse == null)
            {
                var query = async () =>
                {
                    var responseMessage = await _httpClient.GetAsync("account/query-authorizations");
                    
                    responseMessage.RuleOutProblems();
                    responseMessage.EnsureSuccessStatusCode();

                    return (await responseMessage.Content.ReadFromJsonAsync<QueryAuthorizationsResponse>())!;
                };

                _accountHandlerCache.QueryAuthorizationsResponse = query.Invoke();
            }

            var authorizationFilter = _paulaAuthenticationStateManager.GetAuthorizationsFilter();

            return new QueryAuthorizationsResponse
            {
                Values = (await _accountHandlerCache.QueryAuthorizationsResponse)!.Values
                    .Where(x =>
                        authorizationFilter.Any() == false ||
                        authorizationFilter.Contains(x))
                    .ToHashSet()
            };
        }

        public async ValueTask RepairChiefInspectorAsync(RepairChiefInspectorRequest request)
        {
            SetBearerOnHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync("account/repair-chief-inspector", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterInspectorAsync(RegisterInspectorRequest request)
        {
            SetBearerOnHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync("account/register-inspector", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            SetBearerOnHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync("account/register-organization", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<AssessChiefInspectorDefectivenessResponse> AssessChiefInspectorDefectivenessAsync(AssessChiefInspectorDefectivenessRequest request)
        {
            SetBearerOnHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync("account/assess-chief-inspector-defectiveness", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<AssessChiefInspectorDefectivenessResponse>())!;
        }

        public async ValueTask<SignInInspectorResponse> SignInInspectorAsync(SignInInspectorRequest request)
        {
            SetBearerOnHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync("account/sign-in-inspector", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var response = (await responseMessage.Content.ReadFromJsonAsync<SignInInspectorResponse>())!;

            _appState.CurrentInspector = request.UniqueName;
            _appState.CurrentOrganization = request.Organization;

            _accountHandlerCache.FallbackBearer = string.Empty;
            _accountHandlerCache.FallbackInspector = string.Empty;
            _accountHandlerCache.FallbackOrganization = string.Empty;

            _accountHandlerCache.QueryAuthorizationsResponse = null;

            _paulaAuthenticationStateManager.SetAuthenticationBearer(response.Bearer);

            return response;
        }

        public async ValueTask SignOutInspectorAsync()
        {
            SetBearerOnHttpClient();

            _appState.CurrentInspector = string.Empty;
            _appState.CurrentOrganization = string.Empty;

            _accountHandlerCache.FallbackBearer = string.Empty;
            _accountHandlerCache.FallbackInspector = string.Empty;
            _accountHandlerCache.FallbackOrganization = string.Empty;

            _accountHandlerCache.QueryAuthorizationsResponse = null;
            _paulaAuthenticationStateManager.SetAuthenticationBearer(string.Empty);

            var responseMessage = await _httpClient.PostAsync("account/sign-out-inspector", null);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<StartImpersonationResponse> StartImpersonationAsync(StartImpersonationRequest request)
        {
            SetBearerOnHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync("account/start-impersonation", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var response = (await responseMessage.Content.ReadFromJsonAsync<StartImpersonationResponse>())!;

            _accountHandlerCache.FallbackBearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();
            _accountHandlerCache.FallbackInspector = _appState.CurrentInspector;
            _accountHandlerCache.FallbackOrganization = _appState.CurrentOrganization;

            _appState.CurrentInspector = request.UniqueName;
            _appState.CurrentOrganization = request.Organization;

            _accountHandlerCache.QueryAuthorizationsResponse = null;
            _paulaAuthenticationStateManager.SetAuthenticationBearer(response.Bearer);

            return response;
        }

        public ValueTask StopImpersonationAsync()
        {
            SetBearerOnHttpClient();

            if (!string.IsNullOrWhiteSpace(_accountHandlerCache.FallbackBearer))
            {
                _appState.CurrentInspector = _accountHandlerCache.FallbackInspector!;
                _appState.CurrentOrganization = _accountHandlerCache.FallbackOrganization!;

                _accountHandlerCache.QueryAuthorizationsResponse = null;
                _paulaAuthenticationStateManager.SetAuthenticationBearer(_accountHandlerCache.FallbackBearer!);

                _accountHandlerCache.FallbackBearer = string.Empty;
                _accountHandlerCache.FallbackInspector = string.Empty;
                _accountHandlerCache.FallbackOrganization = string.Empty;

            }

            return ValueTask.CompletedTask;
        }
    }
}