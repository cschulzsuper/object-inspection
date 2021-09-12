using Super.Paula.Environment;
using Super.Paula.Web.Shared.Authentication;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Super.Paula.Web.Client.Handling
{
    public class OrganizationHandler : IOrganizationHandler
    {
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly HttpClient _httpClient;

        public OrganizationHandler(
            HttpClient httpClient,
            PaulaAuthenticationStateManager paulaAuthenticationStateManager,
            AppSettings appSettings)
        {
            _paulaAuthenticationStateManager = paulaAuthenticationStateManager;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", _paulaAuthenticationStateManager.GetAuthenticationBearer());
        }

        public async ValueTask ActivateAsync(string organization)
        {
            var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/activate", null);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<OrganizationResponse> CreateAsync(OrganizationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("organizations", request);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<OrganizationResponse>())!;
        }

        public async ValueTask DeactivateAsync(string organization)
        {
            var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/deactivate", null);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask DeleteAsync(string organization)
        {
            var responseMessage = await _httpClient.DeleteAsync($"organizations/{organization}");
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<OrganizationResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("organizations");
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<OrganizationResponse>(
                responseStream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultBufferSize = 128
                });

            await foreach(var reponseItem in response)
            {
                yield return reponseItem!;
            }
        }

        public async ValueTask<OrganizationResponse> GetAsync(string organization)
        {
            var responseMessage = await _httpClient.GetAsync($"organizations/{organization}");
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<OrganizationResponse>())!;
        }

        public async ValueTask ReplaceAsync(string organization, OrganizationRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"organizations/{organization}", request);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
