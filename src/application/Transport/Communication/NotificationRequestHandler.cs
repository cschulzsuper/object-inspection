using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public class NotificationRequestHandler : INotificationRequestHandler
    {
        private readonly INotificationManager _notificationManager;
        private readonly INotificationStreamer _streamer;

        public NotificationRequestHandler(
            INotificationManager notificationManager,
            INotificationStreamer streamer)
        {
            _notificationManager = notificationManager;
            _streamer = streamer;
        }

        public async ValueTask<NotificationResponse> GetAsync(string inspector, int date, int time)
        {
            var entity = await _notificationManager.GetAsync(inspector, date, time);

            return new NotificationResponse
            {
                Date = entity.Date,
                Time = entity.Time,
                Inspector = entity.Inspector,
                Target = entity.Target,
                Text = entity.Text,
                ETag = entity.ETag
            };
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
                    Text = entity.Text,
                    ETag = entity.ETag
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
                        Text = entity.Text,
                        ETag = entity.ETag
                    }));

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
                Target = entity.Target,
                ETag = entity.ETag
            };

            await _streamer.StreamNotificationCreationAsync(response);

            return response;
        }

        public async ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest request)
        {
            var entity = await _notificationManager.GetAsync(inspector, date, time);

            entity.Date = request.Date;
            entity.Time = request.Time;
            entity.Inspector = inspector;
            entity.Target = request.Target;
            entity.Text = request.Text;
            entity.ETag = request.ETag;

            await _notificationManager.UpdateAsync(entity);
        }

        public async ValueTask DeleteAsync(string inspector, int date, int time, string etag)
        {
            var entity = await _notificationManager.GetAsync(inspector, date, time);

            entity.ETag = etag;

            await _notificationManager.DeleteAsync(entity);
            await _streamer.StreamNotificationDeletionAsync(inspector, date, time);
        }
    }
}
