using Super.Paula.Application.Administration;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly INotificationManager _notificationManager;
        
        private Func<NotificationResponse, Task>? _onCreatedHandler;
        private Func<string, int, int, Task>? _onDeletedHandler;

        public NotificationHandler(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public async ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request)
        {
            var entity = new Notification
            {
                Date = request.Date,
                Time = request.Time,
                Inspector = inspector,
                Target = request.Target,
                Text = request.Text
            };

            await _notificationManager.InsertAsync(entity);

            var response = new NotificationResponse
            {
                Date = entity.Date,
                Time = entity.Time,
                Text = entity.Text,
                Inspector = entity.Inspector,
                Target = entity.Target
            };

            var onCreatedTask = _onCreatedHandler?.Invoke(response);
            if (onCreatedTask != null) await onCreatedTask;

            return response;
        }

        public async ValueTask DeleteAsync(string inspector, int date, int time)
        {
            var entity = await _notificationManager.GetAsync(inspector, date, time);

            var onDeletedTask = _onDeletedHandler?.Invoke(inspector, date, time);
            if (onDeletedTask != null) await onDeletedTask;

            await _notificationManager.DeleteAsync(entity);
        }

        public IAsyncEnumerable<NotificationResponse> GetAll()
            => _notificationManager
                .GetAsyncEnumerable(query => query
                .Select(entity => new NotificationResponse
                {
                    Date = entity.Date,
                    Time = entity.Time,
                    Inspector = entity.Inspector,
                    Target = entity.Target,
                    Text = entity.Text
                }));

        public IAsyncEnumerable<NotificationResponse> GetAllForInspector(string inspector)
            => _notificationManager
                .GetInspectorBasedAsyncEnumerable(inspector, query => query
                    .Select(entity => new NotificationResponse
                    {
                        Date = entity.Date,
                        Time = entity.Time,
                        Inspector = entity.Inspector,
                        Target = entity.Target,
                        Text = entity.Text
                    }));

        public async ValueTask<NotificationResponse> GetAsync(string inspector, int date, int time)
        {
            var entity = await _notificationManager.GetAsync(inspector, date, time);

            return new NotificationResponse
            {
                Date = entity.Date,
                Time = entity.Time,
                Inspector = entity.Inspector,
                Target = entity.Target,
                Text = entity.Text
            };
        }

        public Task<IDisposable> OnCreatedAsync(Func<NotificationResponse, Task> handler)
        {
            _onCreatedHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public Task<IDisposable> OnDeletedAsync(Func<string, int, int, Task> handler)
        {
            _onDeletedHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public async ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest request)
        {
            var entity = await _notificationManager.GetAsync(inspector, date, time);

            entity.Date = request.Date;
            entity.Time = request.Time;
            entity.Inspector = inspector;
            entity.Target = request.Target;
            entity.Text = request.Text; 

            await _notificationManager.UpdateAsync(entity);
        }
    }
}
