using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application.Guidlines;
using Super.Paula.Application.Guidlines.Requests;
using Super.Paula.Application.Guidlines.Responses;
using Super.Paula.Client.Authentication;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;

namespace Super.Paula.Client.Guidlines
{
    internal class InspectionHandler : IInspectionHandler
    {
        private readonly HttpClient _httpClient;

        public InspectionHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask ActivateAsync(string inspection)
        {
            var responseMessage = await _httpClient.PostAsync($"inspections/{inspection}/activate", null);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<InspectionResponse> CreateAsync(InspectionRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("inspections", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectionResponse>())!;
        }

        public async ValueTask DeactivateAsync(string inspection)
        {
            var responseMessage = await _httpClient.PostAsync($"inspections/{inspection}/deactivate", null);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask DeleteAsync(string inspection)
        {
            var responseMessage = await _httpClient.DeleteAsync($"inspections/{inspection}");
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<InspectionResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("inspections");
            
            responseMessage.RuleOutProblems();
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
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectionResponse>())!;
        }

        public async ValueTask ReplaceAsync(string inspection, InspectionRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"inspections/{inspection}", request);
            
            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
