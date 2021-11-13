using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Client.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client.Communication
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly INotificationHandler _notificationHandler;

        private readonly ISet<NotificationResponse> _notificationResponsCache;
        private readonly SemaphoreSlim _notificationResponsCacheSemaphore;
        private bool _notificationResponsCached;

        private readonly AuthenticationStateManager _authenticationStateManager;

        public NotificationHandler(
            INotificationHandler notificationHandler,
            AuthenticationStateManager authenticationStateManager)
        {

            _authenticationStateManager = authenticationStateManager;
            _authenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _notificationHandler = notificationHandler;
            _notificationHandler.OnCreatedAsync(InternalOnCreatedAsync);
            _notificationHandler.OnDeletedAsync(InternalOnDeletedAsync);

            _notificationResponsCache = new HashSet<NotificationResponse>();
            _notificationResponsCached = false;
            _notificationResponsCacheSemaphore = new SemaphoreSlim(1, 1);
        }

        private void AuthenticationStateChanged(Task<AuthenticationState> task)
            => task.ContinueWith(async _ =>
            {
                try
                {
                    await _notificationResponsCacheSemaphore.WaitAsync();
                    _notificationResponsCache.Clear();
                    _notificationResponsCached = false;
                }
                finally
                {
                    _notificationResponsCacheSemaphore.Release();
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
                await _notificationResponsCacheSemaphore.WaitAsync();

                if (_notificationResponsCached)
                {
                    foreach (var response in _notificationResponsCache)
                    {
                        yield return response;
                    }
                }
                else
                {
                    var responses = _notificationHandler.GetAllForInspector(inspector);
                    await foreach (var response in responses)
                    {
                        _notificationResponsCache.Add(response);
                        yield return response;
                    }

                    _notificationResponsCached = true;
                }
            }
            finally
            {
                _notificationResponsCacheSemaphore.Release();
            }
        }

        public ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request)
            => throw new NotImplementedException();

        public Task<IDisposable> OnCreatedAsync(Func<NotificationResponse, Task> handler)
            => _notificationHandler.OnCreatedAsync(handler);

        private async Task InternalOnCreatedAsync(NotificationResponse response)
        {
            try
            {
                await _notificationResponsCacheSemaphore.WaitAsync();

                if (_notificationResponsCached)
                {
                    _notificationResponsCache.Add(response);
                }
            }
            finally
            {
                _notificationResponsCacheSemaphore.Release();
            }
        }

        public ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest request)
        {
            throw new NotImplementedException();
        }

        public ValueTask DeleteAsync(string inspector, int date, int time)
            => _notificationHandler.DeleteAsync(inspector, date, time);

        public Task<IDisposable> OnDeletedAsync(Func<string, int, int, Task> handler)
            => _notificationHandler.OnDeletedAsync(handler);

        private async Task InternalOnDeletedAsync(string inspector, int date, int time)
        {
            try
            {
                await _notificationResponsCacheSemaphore.WaitAsync();

                if (_notificationResponsCached)
                {
                    _notificationResponsCache.Remove(
                        _notificationResponsCache.Single(x =>
                            x.Date == date &&
                            x.Time == time &&
                            x.Inspector == inspector));
                }
            }
            finally
            {
                _notificationResponsCacheSemaphore.Release();
            }
        }
    }
}
