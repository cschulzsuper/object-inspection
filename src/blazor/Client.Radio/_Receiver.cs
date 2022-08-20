using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Super.Paula.Shared.Authorization;
using Super.Paula.Shared.Environment;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client;

internal sealed class Receiver : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private readonly AuthenticationStateProvider _AuthenticationStateProvider;

    private readonly SemaphoreSlim _hubConnectionSemaphore;

    private bool _disposed;

    public Receiver(
        AuthenticationStateProvider AuthenticationStateProvider,
        AppSettings appSettings)
    {
        _AuthenticationStateProvider = AuthenticationStateProvider;
        _AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateChangedAsync;

        _hubConnection = new HubConnectionBuilder()
        .WithUrl(
            new Uri(new Uri(appSettings.Server), "/radio"),
            options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    var authenticationState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
                    return authenticationState.User
                        .ToToken()
                        .ToBase64String();
                };
            })
        .WithAutomaticReconnect()
        .Build();

        _hubConnectionSemaphore = new(1);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            _disposed = true;

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
        if (_disposed)
        {
            return;
        }

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
        if (_disposed)
        {
            return;
        }

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

    public async Task<IDisposable> OnAsync<T1, TResult>(string methodName, Func<T1, TResult> handler)
    {
        var subscription = _hubConnection.On(methodName, handler);
        await EnsureStartedAsync();
        return subscription;
    }

    public async Task<IDisposable> OnAsync<T1, T2, TResult>(string methodName, Func<T1, T2, TResult> handler)
    {
        var subscription = _hubConnection.On(methodName, handler);
        await EnsureStartedAsync();
        return subscription;
    }
    public async Task<IDisposable> OnAsync<T1, T2, T3, TResult>(string methodName, Func<T1, T2, T3, TResult> handler)
    {
        var subscription = _hubConnection.On(methodName, handler);
        await EnsureStartedAsync();
        return subscription;
    }
}