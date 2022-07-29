using Super.Paula.Application.Setup;
using Super.Paula.Application.Setup.Requests;
using Super.Paula.Application.Setup.Responses;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Setup
{
    public class ExtensionHandler : IExtensionHandler
    {
        private readonly HttpClient _httpClient;

        public ExtensionHandler(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
        }

        public async ValueTask<ExtensionResponse> CreateAsync(ExtensionRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("extensions", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<ExtensionResponse>())!;
        }

        public async ValueTask<ExtensionFieldCreateResponse> CreateFieldAsync(string type, ExtensionFieldRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"extensions/{type}/fields", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<ExtensionFieldCreateResponse>())!;
        }

        public async ValueTask DeleteAsync(string type, string etag)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"extensions/{type}");
            request.Headers.Add("If-Match", etag);

            var responseMessage = await _httpClient.SendAsync(request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async ValueTask<ExtensionFieldDeleteResponse> DeleteFieldAsync(string type, string field, string etag)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"extensions/{type}/fields/{field}");
            request.Headers.Add("If-Match", etag);

            var responseMessage = await _httpClient.SendAsync(request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<ExtensionFieldDeleteResponse>())!;
        }

        public async IAsyncEnumerable<ExtensionResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync($"extensions");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<ExtensionResponse>(
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

        public async ValueTask<ExtensionResponse> GetAsync(string type)
        {
            var responseMessage = await _httpClient.GetAsync($"extensions/{type}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<ExtensionResponse>())!;
        }
    }
}
