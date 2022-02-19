using Super.Paula.Application.Communication.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public class NotificationAnnouncer : INotificationAnnouncer
    {

        private Func<NotificationResponse, Task>? _onCreationHandler;
        private Func<string, int, int, Task>? _onDeletionHandler;

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

        public async Task AnnounceCreationAsync(NotificationResponse response)
        {
            var onCreationTask = _onCreationHandler?.Invoke(response);
            if (onCreationTask != null) await onCreationTask;
        }

        public async Task AnnounceDeletionAsync(string inspector, int date, int time)
        {
            var onDeletionTask = _onDeletionHandler?.Invoke(inspector, date, time);
            if (onDeletionTask != null) await onDeletionTask;
        }
    }
}
