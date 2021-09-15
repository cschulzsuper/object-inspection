using Super.Paula.Application.Administration;
using Super.Paula.Application.Communication;
using Super.Paula.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ValueTask<Notification> GetAsync(string inspector, int date, int time)
        {
            return _notificationRepository.GetByIdsAsync(inspector, date, time);
        }

        public IQueryable<Notification> GetQueryable()
            => _notificationRepository.GetQueryable();

        public IAsyncEnumerable<Notification> GetAsyncEnumerable()
            => _notificationRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Notification>, IQueryable<TResult>> query)
            => _notificationRepository.GetAsyncEnumerable(query);

        public IQueryable<Notification> GetInspectorBasedQueryable(string inspector)
            => _notificationRepository.GetPartitionQueryable(inspector);

        public IAsyncEnumerable<Notification> GetInspectorBasedAsyncEnumerable(string inspector)
            => _notificationRepository.GetPartitionAsyncEnumerable(inspector);


        public IAsyncEnumerable<TResult> GetInspectorBasedAsyncEnumerable<TResult>(string inspector, Func<IQueryable<Notification>, IQueryable<TResult>> query)
           => _notificationRepository.GetPartitionAsyncEnumerable(query, inspector);


        public ValueTask InsertAsync(Notification notification)
        {
            return _notificationRepository.InsertAsync(notification);
        }

        public ValueTask UpdateAsync(Notification notification)
        {
            return _notificationRepository.UpdateAsync(notification);
        }

        public ValueTask DeleteAsync(Notification notification)
        {
            return _notificationRepository.DeleteAsync(notification);
        }
    }
}
