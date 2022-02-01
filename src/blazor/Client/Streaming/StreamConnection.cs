using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Client.Authentication;
using Super.Paula.Environment;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Client.Streaming
{
    public sealed class StreamConnection : IStreamConnection, IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;
        private readonly AuthenticationStateManager _authenticationStateManager;
        private readonly AppAuthentication _appAuthentication;

        public StreamConnection(
            AuthenticationStateManager authenticationStateManager,
            AppAuthentication appAuthentication,
            AppSettings appSettings)
        {
            _authenticationStateManager = authenticationStateManager;
            _authenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _appAuthentication = appAuthentication;

            _hubConnection = new HubConnectionBuilder()
            .WithUrl(
                new Uri(new Uri(appSettings.Server), "/stream"),
                c =>
                {
                    c.AccessTokenProvider = () => Task.FromResult(_appAuthentication.Token)!;
                })
            .Build();


        }

        public async ValueTask DisposeAsync()
        {
            _authenticationStateManager.AuthenticationStateChanged -= AuthenticationStateChanged;

            await _hubConnection.DisposeAsync();
        }

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
                if (_appAuthentication.Authorizations.Any())
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

        public async Task<IDisposable> OnNotificationCreationAsync(Func<NotificationResponse, Task> handler)
        {
            var onCreated = _hubConnection.On("NotificationCreation", handler);
            await StartHubAsync();
            return onCreated;
        }

        public async Task<IDisposable> OnNotificationDeletionAsync(Func<string, int, int, Task> handler)
        {
            var onDeleted = _hubConnection.On("NotificationDeletion", handler);
            await StartHubAsync();
            return onDeleted;
        }
    }
}
