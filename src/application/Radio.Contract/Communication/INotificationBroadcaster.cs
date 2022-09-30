using ChristianSchulz.ObjectInspection.Application.Communication.Responses;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Communication;

public interface INotificationBroadcaster
{
    Task SendNotificationCreationAsync(NotificationResponse response);

    Task SendNotificationDeletionAsync(string inspector, int date, int time);
}