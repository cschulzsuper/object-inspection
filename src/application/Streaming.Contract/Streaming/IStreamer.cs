using Super.Paula.Application.Communication.Responses;
using Super.Paula.Application.Inventory.Events;
using System.Threading.Tasks;

namespace Super.Paula.Application.Streaming
{
    public interface IStreamer
    {
        Task StreamNotificationCreationAsync(NotificationResponse response);

        Task StreamNotificationDeletionAsync(string inspector, int date, int time);

        Task StreamInspectorBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response);

        Task StreamInspectorBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response);

        Task StreamInspectorBusinessObjectDeletionAsync(string inspector, string businessObject);
    }
}
