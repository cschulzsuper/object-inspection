using Super.Paula.Application.Communication.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public interface INotificationStreamer
    {
        Task StreamNotificationCreationAsync(NotificationResponse response);

        Task StreamNotificationDeletionAsync(string inspector, int date, int time);
    }
}
