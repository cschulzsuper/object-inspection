using Microsoft.AspNetCore.SignalR.Client;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Environment;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Streaming
{
    public sealed class RemoteStreamer : IStreamer, IDisposable
    {
        private readonly HubConnection _connection;
        private readonly AppSettings _appSettings;
        private readonly AppState _appState;

        public RemoteStreamer(AppSettings appSettings, AppState appState)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(
                    new Uri(new Uri(appSettings.Server), "/stream"),
                    options => options.AccessTokenProvider = CreateTokenAsync)
                .Build();

            _appSettings = appSettings;
            _appState = appState;
        }
        public void Dispose()
        {
            _ = _connection.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public Task<string?> CreateTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(_appSettings.StreamerSecret))
            {
                Task.FromResult((string?)null);
            }

            var token = new Token
            {
                StreamerSecret = _appSettings.StreamerSecret
            };

            return Task.FromResult(token.ToBase64String())!;
        }

        public async Task EnsureStartedAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }
        }

        public async Task StreamNotificationCreationAsync(NotificationResponse response)
        {
            await EnsureStartedAsync();
            var userId = $"{_appState.CurrentOrganization}:{response.Inspector}";
            await _connection.SendAsync("Stream1", userId, "NotificationCreation", response);
        }

        public async Task StreamNotificationDeletionAsync(string inspector, int date, int time)
        {
            await EnsureStartedAsync();
            var userId = $"{_appState.CurrentOrganization}:{inspector}";
            await _connection.SendAsync("Stream3", userId, "NotificationDeletion", inspector, date, time);
        }
    }
}
