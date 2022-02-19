using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class OrganizationHandler : IOrganizationHandler
    {
        private readonly HttpClient _httpClient;

        public OrganizationHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask ActivateAsync(string organization)
        {
            var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/activate", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<OrganizationResponse> CreateAsync(OrganizationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("organizations", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<OrganizationResponse>())!;
        }

        public async ValueTask DeactivateAsync(string organization)
        {
            var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/deactivate", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask DeleteAsync(string organization)
        {
            var responseMessage = await _httpClient.DeleteAsync($"organizations/{organization}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<OrganizationResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("organizations");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<OrganizationResponse>(
                responseStream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultBufferSize = 128
                });

            await foreach (var responseItem in response)
            {
                yield return responseItem!;
            }
        }

        public async ValueTask<OrganizationResponse> GetAsync(string organization)
        {
            var responseMessage = await _httpClient.GetAsync($"organizations/{organization}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<OrganizationResponse>())!;
        }

        public async ValueTask ReplaceAsync(string organization, OrganizationRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"organizations/{organization}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
