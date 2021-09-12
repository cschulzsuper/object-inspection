using Super.Paula.Environment;
using Super.Paula.Web.Shared.Authentication;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Web.Client.Handling
{
    public class InspectionHandler : IInspectionHandler
    {
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly HttpClient _httpClient;

        public InspectionHandler(
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

        public async ValueTask ActivateAsync(string inspection)
        {
            var responseMessage = await _httpClient.PostAsync($"inspections/{inspection}/activate", null);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<InspectionResponse> CreateAsync(InspectionRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("inspections", request);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectionResponse>())!;
        }

        public async ValueTask DeactivateAsync(string inspection)
        {
            var responseMessage = await _httpClient.PostAsync($"inspections/{inspection}/deactivate", null);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask DeleteAsync(string inspection)
        {
            var responseMessage = await _httpClient.DeleteAsync($"inspections/{inspection}");
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<InspectionResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("inspections");
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<InspectionResponse>(
                responseStream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultBufferSize = 128
                });

            await foreach (var reponseItem in response)
            {
                yield return reponseItem!;
            }
        }

        public async ValueTask<InspectionResponse> GetAsync(string inspection)
        {
            var responseMessage = await _httpClient.GetAsync($"inspections/{inspection}");
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectionResponse>())!;
        }

        public async ValueTask ReplaceAsync(string inspection, InspectionRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"inspections/{inspection}", request);
            
            responseMessage.RuleOutClientError(_ClientErrors.ProblemDetailsTitle);
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
