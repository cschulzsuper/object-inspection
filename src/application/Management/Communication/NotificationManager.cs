using Super.Paula.Application.Administration;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidlines;
using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application
{
    public class NotificationManager : INotificationManager
    {
        private readonly IRepository<Notification> _notificationRepository;

        public NotificationManager(IRepository<Notification> notification)
        {
            _notificationRepository = notification;
        }
        public async ValueTask<Notification> GetAsync(string inspector, int date, int time)
        {
            EnsureGetable(inspector, date, time);

            var notification = await _notificationRepository.GetByIdsOrDefaultAsync(inspector, date, time);
            if (notification == null)
            {
                throw new ManagementException($"Notification '{inspector}/{date}/{time}' was not found");
            }

            return notification;
        }

        public IQueryable<Notification> GetQueryable()
            => _notificationRepository.GetQueryable();

        public IAsyncEnumerable<Notification> GetAsyncEnumerable()
            => _notificationRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Notification>, IQueryable<TResult>> query)
            => _notificationRepository.GetAsyncEnumerable(query);

        public IQueryable<Notification> GetInspectorBasedQueryable(string inspector)
        {
            EnsureGetableInspectorBased(inspector);
            return _notificationRepository.GetPartitionQueryable(inspector);
        }

        public IAsyncEnumerable<Notification> GetInspectorBasedAsyncEnumerable(string inspector)
        {
            EnsureGetableInspectorBased(inspector);
            return _notificationRepository.GetPartitionAsyncEnumerable(inspector);
        }

        public IAsyncEnumerable<TResult> GetInspectorBasedAsyncEnumerable<TResult>(string inspector, Func<IQueryable<Notification>, IQueryable<TResult>> query)
        {
            EnsureGetableInspectorBased(inspector);
            return _notificationRepository.GetPartitionAsyncEnumerable(query, inspector);
        }

        public async ValueTask InsertAsync(Notification notification)
        {
            EnsureInsertable(notification);

            try
            {
                await _notificationRepository.InsertAsync(notification);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert notification '{notification.Inspector}/{notification.Date}/{notification.Time}'", exception);
            }
        }

        public async ValueTask UpdateAsync(Notification notification)
        {
            EnsureUpdatable(notification);

            try
            {
                await _notificationRepository.UpdateAsync(notification);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update notification '{notification.Inspector}/{notification.Date}/{notification.Time}'", exception);
            }
        }

        public async ValueTask DeleteAsync(Notification notification)
        {
            EnsureDeletable(notification);

            try
            {
                await _notificationRepository.DeleteAsync(notification);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete notification '{notification.Inspector}/{notification.Date}/{notification.Time}'", exception);
            }
        }

        private static void EnsureGetable(string inspector, int date, int time)
            => Validator.Ensure($"id '{inspector}/{date}/{time}' for notification",
                NotificationValidator.DateIsPositive(date),
                NotificationValidator.TimeIsInDayTimeRange(time),
                NotificationValidator.InspectorIsNotEmpty(inspector),
                NotificationValidator.InspectorHasKebabCase(inspector),
                NotificationValidator.InspectorIsNotTooLong(inspector));

        private static void EnsureGetableInspectorBased(string inspector)
            => Validator.Ensure($"inspector for notifications",
                NotificationValidator.InspectorIsNotEmpty(inspector),
                NotificationValidator.InspectorHasKebabCase(inspector),
                NotificationValidator.InspectorIsNotTooLong(inspector));

        private static void EnsureInsertable(Notification notification)
            => Validator.Ensure($"notification with id '{notification.Inspector}/{notification.Date}/{notification.Time}'",
                NotificationValidator.DateIsPositive(notification.Date),
                NotificationValidator.TimeIsInDayTimeRange(notification.Time),
                NotificationValidator.InspectorIsNotEmpty(notification.Inspector),
                NotificationValidator.InspectorHasKebabCase(notification.Inspector),
                NotificationValidator.InspectorIsNotTooLong(notification.Inspector),
                NotificationValidator.TargetIsNotEmpty(notification.Target),
                NotificationValidator.TargetIsRelativeUri(notification.Target),
                NotificationValidator.TargetIsNotTooLong(notification.Target),
                NotificationValidator.TextHasValue(notification.Text),
                NotificationValidator.TextIsNotTooLong(notification.Text));

        private static void EnsureUpdatable(Notification notification)
            => Validator.Ensure($"notification with id '{notification.Inspector}/{notification.Date}/{notification.Time}'",
                NotificationValidator.DateIsPositive(notification.Date),
                NotificationValidator.TimeIsInDayTimeRange(notification.Time),
                NotificationValidator.InspectorIsNotEmpty(notification.Inspector),
                NotificationValidator.InspectorHasKebabCase(notification.Inspector),
                NotificationValidator.InspectorIsNotTooLong(notification.Inspector),
                NotificationValidator.TargetIsNotEmpty(notification.Target),
                NotificationValidator.TargetIsRelativeUri(notification.Target),
                NotificationValidator.TargetIsNotTooLong(notification.Target),
                NotificationValidator.TextHasValue(notification.Text),
                NotificationValidator.TextIsNotTooLong(notification.Text));

        private static void EnsureDeletable(Notification notification)
            => Validator.Ensure($"notification with id '{notification.Inspector}/{notification.Date}/{notification.Time}'",
                NotificationValidator.DateIsPositive(notification.Date),
                NotificationValidator.TimeIsInDayTimeRange(notification.Time),
                NotificationValidator.InspectorIsNotEmpty(notification.Inspector),
                NotificationValidator.InspectorHasKebabCase(notification.Inspector),
                NotificationValidator.InspectorIsNotTooLong(notification.Inspector));
    }
}
