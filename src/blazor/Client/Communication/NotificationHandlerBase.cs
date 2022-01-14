using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Client.Authentication;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Environment;

namespace Super.Paula.Client.Communication
{
    public sealed class NotificationHandlerBase : INotificationHandler, IAsyncDisposable
    {
        private readonly AuthenticationStateManager _authenticationStateManager;
        private readonly AppAuthentication _appAuthentication;
        private readonly AppSettings _appSettings;
        private readonly IAccountHandler _accountHandler;

        private readonly HttpClient _httpClient;      
        private readonly HubConnection _hubConnection;

        public NotificationHandlerBase(
            HttpClient httpClient,
            AuthenticationStateManager authenticationStateManager,
            AppAuthentication appAuthentication,
            AppSettings appSettings,
            IAccountHandler accountHandler)
        {
            _accountHandler = accountHandler;
            _appSettings = appSettings;

            _authenticationStateManager = authenticationStateManager;
            _appAuthentication = appAuthentication;
            _authenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_appSettings.Server);

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(
                    new Uri(_httpClient.BaseAddress, "/notifications/signalr"),
                    c => {
                        c.AccessTokenProvider = () => Task.FromResult(_appAuthentication.Token)!;
                    })
                .Build();
        }

        public ValueTask DisposeAsync()
            => _hubConnection.DisposeAsync();

        private void AuthenticationStateChanged(Task<AuthenticationState> task)
            => task.ContinueWith(async _ =>
                {
                    await StopHubAsync();
                    await StartHubAsync();
                });

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

        public async ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"inspectors/{inspector}/notifications", request);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();

            return (await responseMessage.Content.ReadFromJsonAsync<NotificationResponse>())!;
        }

        public async Task<IDisposable> OnCreatedAsync(Func<NotificationResponse, Task> handler)
        {
            var onCreated = _hubConnection.On("OnCreated", handler);
            await StartHubAsync();
            return onCreated;
        }

        public async Task<IDisposable> OnDeletedAsync(Func<string, int, int, Task> handler)
        {
            var onDeleted = _hubConnection.On("OnDeleted", handler);
            await StartHubAsync();
            return onDeleted;
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
