using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Client.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client.Communication
{
    public class NotificationHandler : INotificationHandler, IDisposable
    {
        private readonly INotificationHandler _notificationHandler;

        private readonly ISet<NotificationResponse> _notificationResponseCache;
        private readonly SemaphoreSlim _notificationResponseCacheSemaphore;
        private bool _notificationResponseCached;

        private readonly AuthenticationStateManager _authenticationStateManager;

        public NotificationHandler(
            INotificationHandler notificationHandler,
            AuthenticationStateManager authenticationStateManager)
        {

            _authenticationStateManager = authenticationStateManager;
            _authenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _notificationHandler = notificationHandler;
            _notificationHandler.OnCreationAsync(InternalOnCreationAsync);
            _notificationHandler.OnDeletionAsync(InternalOnDeletionAsync);

            _notificationResponseCache = new HashSet<NotificationResponse>();
            _notificationResponseCached = false;
            _notificationResponseCacheSemaphore = new SemaphoreSlim(1, 1);
        }

        public void Dispose()
        {
            _authenticationStateManager.AuthenticationStateChanged -= AuthenticationStateChanged;

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
                    var responses = _notificationHandler.GetAllForInspector(inspector);
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

        public Task<IDisposable> OnCreationAsync(Func<NotificationResponse, Task> handler)
            => _notificationHandler.OnCreationAsync(handler);

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

        public ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest request)
        {
            throw new NotImplementedException();
        }

        public ValueTask DeleteAsync(string inspector, int date, int time)
            => _notificationHandler.DeleteAsync(inspector, date, time);

        public Task<IDisposable> OnDeletionAsync(Func<string, int, int, Task> handler)
            => _notificationHandler.OnDeletionAsync(handler);

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
}
