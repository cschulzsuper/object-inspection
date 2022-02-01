using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Super.Paula.Application.Inventory.Events;

namespace Super.Paula.Application.Communication
{
    public class NotificationHandler : INotificationHandler, INotificationEventHandler
    {
        private readonly INotificationManager _notificationManager;
        
        private Func<NotificationResponse, Task>? _onCreationHandler;
        private Func<string, int, int, Task>? _onDeletionHandler;

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

            var onCreationTask = _onCreationHandler?.Invoke(response);
            if (onCreationTask != null) await onCreationTask;

            return response;
        }

        public async ValueTask DeleteAsync(string inspector, int date, int time)
        {
            var entity = await _notificationManager.GetAsync(inspector, date, time);

            var onDeletionTask = _onDeletionHandler?.Invoke(inspector, date, time);
            if (onDeletionTask != null) await onDeletionTask;

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

        public Task<IDisposable> OnCreationAsync(Func<NotificationResponse, Task> handler)
        {
            _onCreationHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public Task<IDisposable> OnDeletionAsync(Func<string, int, int, Task> handler)
        {
            _onDeletionHandler = handler;
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

        public async ValueTask ProcessAsync(string businessObject, BusinessObjectInspectorEvent @event)
        {
            var (date, time) = DateTime.UtcNow.ToNumbers();

            if (@event.OldInspector != null &&
                @event.OldInspector != @event.NewInspector)
            {

                await CreateAsync(@event.OldInspector, new NotificationRequest
                {
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{businessObject}",
                    Text = $"You are not longer the inspector for {@event.BusinessObjectDisplayName}!"
                });
            }

            if (@event.NewInspector != null &&
                @event.NewInspector != @event.OldInspector)
            {

                await CreateAsync(@event.NewInspector, new NotificationRequest
                {
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{businessObject}",
                    Text = $"You are now the inspector for {@event.BusinessObjectDisplayName}!"
                });
            }
        }

        public async ValueTask ProcessAsync(string inspector, InspectorBusinessObjectEvent @event)
        {
            if (@event.NewAuditSchedulePending == true &&
                @event.NewAuditSchedulePending != @event.OldAuditSchedulePending &&
                @event.NewAuditScheduleDelayed == false &&
                @event.OldAuditScheduleDelayed == false)
            {

                var (date, time) = DateTime.UtcNow.ToNumbers();

                await CreateAsync(inspector, new NotificationRequest
                {
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{@event.UniqueName}",
                    Text = $"An inspection audit for {@event.DisplayName} imminent!"
                });
            }

            if (@event.NewAuditScheduleDelayed == true &&
                @event.NewAuditScheduleDelayed != @event.OldAuditScheduleDelayed)
            {

                var (date, time) = DateTime.UtcNow.ToNumbers();

                await CreateAsync(inspector, new NotificationRequest
                {
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{@event.UniqueName}",
                    Text = $"An inspection audit for {@event.DisplayName} overdue!"
                });
            }
        }
    }
}
