using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
    internal class InspectorHandler : IInspectorHandler
    {
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly HttpClient _httpClient;

        public InspectorHandler(
            HttpClient httpClient,
            PaulaAuthenticationStateManager paulaAuthenticationStateManager,
            AppSettings appSettings)
        {
            _paulaAuthenticationStateManager = paulaAuthenticationStateManager;
            _paulaAuthenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
            SetBearerOnHttpClient();
        }

        private void AuthenticationStateChanged(Task<AuthenticationState> task)
            => task.ContinueWith(_ =>
            {
                SetBearerOnHttpClient();
            });

        private void SetBearerOnHttpClient()
        {
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();

            _httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrWhiteSpace(bearer)
                    ? new AuthenticationHeaderValue("Bearer", bearer)
                    : null;
        }

        public async ValueTask ActivateAsync(string inspector)
        {
            var responseMessage = await _httpClient.PostAsync($"inspectors/{inspector}/activate", null);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("inspectors", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectorResponse>())!;
        }

        public async ValueTask DeactivateAsync(string inspector)
        {
            var responseMessage = await _httpClient.PostAsync($"inspectors/{inspector}/deactivate", null);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask DeleteAsync(string inspector)
        {
            var responseMessage = await _httpClient.DeleteAsync($"inspectors/{inspector}");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<InspectorResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("inspectors");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<InspectorResponse>(
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

        public async IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
        {
            var responseMessage = await _httpClient.GetAsync($"organizations/{organization}/inspectors");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<InspectorResponse>(
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

        public async ValueTask<InspectorResponse> GetAsync(string inspector)
        {
            var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectorResponse>())!;
        }

        public ValueTask RefreshOrganizationAsync(string organization, RefreshOrganizationRequest request)
            => throw new NotSupportedException("This operation is not supported on the client side");

        public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"inspectors/{inspector}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
