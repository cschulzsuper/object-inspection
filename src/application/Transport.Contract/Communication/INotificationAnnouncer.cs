using Super.Paula.Application.Communication.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public interface INotificationAnnouncer
    {

        Task<IDisposable> OnCreationAsync(Func<NotificationResponse, Task> handler);

        Task<IDisposable> OnDeletionAsync(Func<string, int, int, Task> handler);

        Task AnnounceCreationAsync(NotificationResponse response);

        Task AnnounceDeletionAsync(string inspector, int date, int time);
    }
}