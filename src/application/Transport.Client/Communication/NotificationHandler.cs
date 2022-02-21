using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Client.Streaming;
using Super.Paula.Environment;
using Super.Paula.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Super.Paula.Client.Communication
{
    public sealed class NotificationHandler : INotificationHandler
    {
        private readonly HttpClient _httpClient;

        private readonly IStreamConnection _streamConnection;

        public NotificationHandler(
            HttpClient httpClient,
            AppSettings appSettings,
            IStreamConnection streamConnection)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Server);
            _streamConnection = streamConnection;
        }

        public async ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"inspectors/{inspector}/notifications", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<NotificationResponse>())!;
        }

        public Task<IDisposable> OnCreationAsync(Func<NotificationResponse, Task> handler)
            => _streamConnection.OnNotificationCreationAsync(handler);

        public Task<IDisposable> OnDeletionAsync(Func<string, int, int, Task> handler)
            => _streamConnection.OnNotificationDeletionAsync(handler);

        public async ValueTask DeleteAsync(string inspector, int date, int time)
        {
            var responseMessage = await _httpClient.DeleteAsync($"inspectors/{inspector}/notifications/{date}/{time}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }

        public async IAsyncEnumerable<NotificationResponse> GetAll()
        {
            var responseMessage = await _httpClient.GetAsync("notifications");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<NotificationResponse>(
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

        public async IAsyncEnumerable<NotificationResponse> GetAllForInspector(string inspector)
        {
            var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}/notifications");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            var responseStream = await responseMessage.Content.ReadAsStreamAsync();
            var response = JsonSerializer.DeserializeAsyncEnumerable<NotificationResponse>(
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

        public async ValueTask<NotificationResponse> GetAsync(string inspector, int date, int time)
        {
            var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}/notifications/{date}/{time}");

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<NotificationResponse>())!;
        }

        public async ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"inspectors/{inspector}/notifications/{date}/{time}", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
