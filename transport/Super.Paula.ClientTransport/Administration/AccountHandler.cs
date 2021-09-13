using System.Net.Http.Headers;
using System.Net.Http.Json;
using Super.Paula.Administration.Requests;
using Super.Paula.Administration.Responses;
using Super.Paula.Authentication;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;

namespace Super.Paula.Administration
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
            _accountHandlerCache = accountHandlerCache;
            _paulaAuthenticationStateManager = paulaAuthenticationStateManager;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);

            _appState = appState;
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var responseMessage = await _httpClient.PostAsJsonAsync("account/account/change-secret", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync()
        {
            if (_accountHandlerCache.QueryAuthorizationsResponse == null)
            {
                var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();
                _httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrWhiteSpace(bearer)
                    ? new AuthenticationHeaderValue("Bearer", bearer)
                    : null;

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
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();
            if (!string.IsNullOrWhiteSpace(bearer))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            }

            var responseMessage = await _httpClient.PostAsJsonAsync("account/repair-chief-inspector", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask RegisterInspectorAsync(RegisterInspectorRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("account/register-inspector", request);
            
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
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();
            if (!string.IsNullOrWhiteSpace(bearer))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            }

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
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();
            if (!string.IsNullOrWhiteSpace(bearer))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            }

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
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();
            if (!string.IsNullOrWhiteSpace(bearer))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            }

            var responseMessage = await _httpClient.PostAsJsonAsync("account/start-impersonation", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var response = (await responseMessage.Content.ReadFromJsonAsync<StartImpersonationResponse>())!;

            _accountHandlerCache.FallbackBearer = bearer;
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