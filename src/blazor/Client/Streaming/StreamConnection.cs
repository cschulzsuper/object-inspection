using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Authorization;
using Super.Paula.Environment;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client.Streaming
{
    public sealed class StreamConnection : IStreamConnection, IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;
        private readonly AuthenticationStateProvider _AuthenticationStateProvider;

        private readonly SemaphoreSlim _hubConnectionSemaphore;

        public StreamConnection(
            AuthenticationStateProvider AuthenticationStateProvider,
            AppSettings appSettings)
        {
            _AuthenticationStateProvider = AuthenticationStateProvider;
            _AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateChangedAsync;

            _hubConnection = new HubConnectionBuilder()
            .WithUrl(
                new Uri(new Uri(appSettings.Server), "/stream"),
                c =>
                {
                    c.AccessTokenProvider = async () =>
                    {
                        var authenticationState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
                        return authenticationState.User
                            .ToToken()
                            .ToBase64String();
                    };
                })
            .Build();

            _hubConnectionSemaphore = new SemaphoreSlim(1, 1);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await _hubConnectionSemaphore.WaitAsync();

                _AuthenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateChangedAsync;

                await _hubConnection.DisposeAsync();
            }
            finally
            {
                _hubConnectionSemaphore.Release();
            }
        }

        private async void AuthenticationStateChangedAsync(Task<AuthenticationState> authenticationState)
        {
            await authenticationState;
            await EnsureStoppedAsync();
            await EnsureStartedAsync();
        }

        private async Task EnsureStartedAsync()
        {
            try
            {
                await _hubConnectionSemaphore.WaitAsync();

                if (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    var authenticationState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
                    if (authenticationState.User.Identity?.IsAuthenticated == true)
                    {
                        await _hubConnection.StartAsync();
                    }
                }
            }
            finally
            {
                _hubConnectionSemaphore.Release();
            }
        }

        private async Task EnsureStoppedAsync()
        {
            try
            {
                await _hubConnectionSemaphore.WaitAsync();

                if (_hubConnection.State != HubConnectionState.Disconnected)
                {
                    await _hubConnection.StopAsync();
                }
            }
            finally
            {
                _hubConnectionSemaphore.Release();
            }
        }

        public async Task<IDisposable> OnNotificationCreationAsync(Func<NotificationResponse, Task> handler)
        {
            var subscription = _hubConnection.On("NotificationCreation", handler);
            await EnsureStartedAsync();
            return subscription;
        }

        public async Task<IDisposable> OnNotificationDeletionAsync(Func<string, int, int, Task> handler)
        {
            var subscription = _hubConnection.On("NotificationDeletion", handler);
            await EnsureStartedAsync();
            return subscription;
        }

        public async Task<IDisposable> OnInspectorBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        {
            var subscription = _hubConnection.On("InspectorBusinessObjectCreation", handler);
            await EnsureStartedAsync();
            return subscription;
        }

        public async Task<IDisposable> OnInspectorBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        {
            var subscription = _hubConnection.On("InspectorBusinessObjectUpdate", handler);
            await EnsureStartedAsync();
            return subscription;
        }

        public async Task<IDisposable> OnInspectorBusinessObjectDeletionAsync(Func<string, string, Task> handler)
        {
            var subscription = _hubConnection.On("InspectorBusinessObjectDeletion", handler);
            await EnsureStartedAsync();
            return subscription;
        }
    }
}
