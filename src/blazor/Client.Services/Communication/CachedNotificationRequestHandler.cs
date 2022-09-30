using Microsoft.AspNetCore.Components.Authorization;
using ChristianSchulz.ObjectInspection.Application.Communication;
using ChristianSchulz.ObjectInspection.Application.Communication.Requests;
using ChristianSchulz.ObjectInspection.Application.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Communication;

public class CachedNotificationRequestHandler : INotificationRequestHandler, IDisposable
{
    private readonly INotificationRequestHandler _notificationRequestHandler;
    private readonly INotificationCallbackHandler _notificationCallbackHandler;

    private readonly ISet<NotificationResponse> _notificationResponseCache;
    private readonly SemaphoreSlim _notificationResponseCacheSemaphore;
    private bool _notificationResponseCached;

    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public CachedNotificationRequestHandler(
        INotificationRequestHandler notificationRequestHandler,
        INotificationCallbackHandler notificationCallbackHandler,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _authenticationStateProvider.AuthenticationStateChanged += AuthenticationStateChanged;

        _notificationRequestHandler = notificationRequestHandler;
        _notificationCallbackHandler = notificationCallbackHandler;
        _notificationCallbackHandler.OnCreationAsync(InternalOnCreationAsync);
        _notificationCallbackHandler.OnDeletionAsync(InternalOnDeletionAsync);

        _notificationResponseCache = new HashSet<NotificationResponse>();
        _notificationResponseCached = false;
        _notificationResponseCacheSemaphore = new(1);
    }

    public void Dispose()
    {
        _authenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateChanged;

        GC.SuppressFinalize(this);
    }

    private void AuthenticationStateChanged(Task<AuthenticationState> task)
        => task.ContinueWith(async _ =>
        {
            try
            {
                await _notificationResponseCacheSemaphore.WaitAsync();
                _notificationResponseCache.Clear();
                _notificationResponseCached = false;
            }
            finally
            {
                _notificationResponseCacheSemaphore.Release();
            }
        });

    public ValueTask<NotificationResponse> GetAsync(string inspector, int date, int time)
        => throw new NotImplementedException();

    public IAsyncEnumerable<NotificationResponse> GetAll()
        => throw new NotImplementedException();

    public async IAsyncEnumerable<NotificationResponse> GetAllForInspector(string inspector)
    {
        try
        {
            await _notificationResponseCacheSemaphore.WaitAsync();

            if (_notificationResponseCached)
            {
                foreach (var response in _notificationResponseCache)
                {
                    yield return response;
                }
            }
            else
            {
                var responses = _notificationRequestHandler.GetAllForInspector(inspector);
                await foreach (var response in responses)
                {
                    _notificationResponseCache.Add(response);
                    yield return response;
                }

                _notificationResponseCached = true;
            }
        }
        finally
        {
            _notificationResponseCacheSemaphore.Release();
        }
    }

    public ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request)
        => throw new NotImplementedException();

    public ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest requestg)
    {
        throw new NotImplementedException();
    }

    public ValueTask DeleteAsync(string inspector, int date, int time, string etag)
        => _notificationRequestHandler.DeleteAsync(inspector, date, time, etag);

    private async Task InternalOnCreationAsync(NotificationResponse response)
    {
        try
        {
            await _notificationResponseCacheSemaphore.WaitAsync();

            if (_notificationResponseCached)
            {
                _notificationResponseCache.Add(response);
            }
        }
        finally
        {
            _notificationResponseCacheSemaphore.Release();
        }
    }

    private async Task InternalOnDeletionAsync(string inspector, int date, int time)
    {
        try
        {
            await _notificationResponseCacheSemaphore.WaitAsync();

            if (_notificationResponseCached)
            {
                _notificationResponseCache.Remove(
                    _notificationResponseCache.Single(x =>
                        x.Date == date &&
                        x.Time == time &&
                        x.Inspector == inspector));
            }
        }
        finally
        {
            _notificationResponseCacheSemaphore.Release();
        }
    }
}