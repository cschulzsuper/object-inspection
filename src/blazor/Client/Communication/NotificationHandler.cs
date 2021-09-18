using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Authentication;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;

namespace Super.Paula.Client.Communication
{
    internal class NotificationHandler : INotificationHandler, IAsyncDisposable
    {
        private readonly PaulaAuthenticationStateManager _paulaAuthenticationStateManager;
        private readonly AppSettings _appSettings;
        private readonly IAccountHandler _accountHandler;

        private readonly HttpClient _httpClient;      
        private readonly HubConnection _hubConnection;

        public NotificationHandler(
            HttpClient httpClient,
            PaulaAuthenticationStateManager paulaAuthenticationStateManager,
            AppSettings appSettings,
            IAccountHandler accountHandler)
        {
            _accountHandler = accountHandler;
            _appSettings = appSettings;

            _paulaAuthenticationStateManager = paulaAuthenticationStateManager;
            _paulaAuthenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_appSettings.Server);
            SetBearerOnHttpClient();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(
                    new Uri(_httpClient.BaseAddress, "/notifications/live"),
                    c => {
                        c.AccessTokenProvider = () => Task.FromResult(_paulaAuthenticationStateManager.GetAuthenticationBearer())!;
                    })
                .Build();
        }

        public ValueTask DisposeAsync()
            => _hubConnection.DisposeAsync();

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

        private void AuthenticationStateChanged(Task<AuthenticationState> task)
            => task.ContinueWith(
                async _ =>
                {
                    SetBearerOnHttpClient();
                    await StopHubAsync();
                    await StartHubAsync();
                });

        public async Task<IDisposable> OnCreatedAsync(Func<NotificationResponse, Task> handler)
        {
            var onCreated = _hubConnection.On("OnCreated", handler);
            await StartHubAsync();
            return onCreated;
        }

        private void SetBearerOnHttpClient()
        {
            var bearer = _paulaAuthenticationStateManager.GetAuthenticationBearer();

            _httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrWhiteSpace(bearer)
                    ? new AuthenticationHeaderValue("Bearer", bearer)
                    : null;
        }

        private async Task StartHubAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                if ((await _accountHandler.QueryAuthorizationsAsync()).Values.Any())
                {
                    await _hubConnection.StartAsync();
                }
            }
        }

        private async Task StopHubAsync()
        {
            if (_hubConnection.State != HubConnectionState.Disconnected)
            {
                await _hubConnection.StopAsync();
            }
        }

    }
}
