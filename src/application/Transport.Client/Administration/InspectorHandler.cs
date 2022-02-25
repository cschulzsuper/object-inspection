using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Client.Streaming;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class InspectorHandler : IInspectorHandler
    {
        private readonly HttpClient _httpClient;

        private readonly IStreamConnection _streamConnection;

        public InspectorHandler(
            HttpClient httpClient,
            AppSettings appSettings,
            IStreamConnection streamConnection)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);

            _streamConnection = streamConnection;
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

            await foreach (var responseItem in response)
            {
                yield return responseItem!;
            }
        }

        public async IAsyncEnumerable<IdentityInspectorResponse> GetAllForIdentity(string identity)
        {
            var responseMessage = await _httpClient.GetAsync($"identities/{identity}/inspectors");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<IdentityInspectorResponse>(
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

            await foreach (var responseItem in response)
            {
                yield return responseItem!;
            }
        }

        public async ValueTask<InspectorResponse> GetAsync(string inspector)
        {
            var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectorResponse>())!;
        }

        public async ValueTask<InspectorResponse> GetCurrentAsync()
        {
            var responseMessage = await _httpClient.GetAsync($"inspectors/me");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<InspectorResponse>())!;
        }

        public Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
            => _streamConnection.OnInspectorBusinessObjectCreationAsync(handler);

        public Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler)
            => _streamConnection.OnInspectorBusinessObjectDeletionAsync(handler);

        public Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
            => _streamConnection.OnInspectorBusinessObjectUpdateAsync(handler);

        public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync($"inspectors/{inspector}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
