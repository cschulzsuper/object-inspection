using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Authentication;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;

namespace Super.Paula.Client.Communication
{
    internal class NotificationHandler : INotificationHandler
    {
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly HttpClient _httpClient;

        public NotificationHandler(
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

        public async ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"inspectors/{inspector}/notifications", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<NotificationResponse>())!;
        }

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

            await foreach (var reponseItem in response)
            {
                yield return reponseItem!;
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

            await foreach (var reponseItem in response)
            {
                yield return reponseItem!;
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
