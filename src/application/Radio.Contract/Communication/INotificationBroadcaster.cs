using Super.Paula.Application.Communication.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication;

public interface INotificationBroadcaster
{
    Task SendNotificationCreationAsync(NotificationResponse response);

    Task SendNotificationDeletionAsync(string inspector, int date, int time);
}