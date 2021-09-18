using Super.Paula.Application.Communication.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public interface INotificationMessenger
    {
        Task OnCreatedAsync(NotificationResponse response);

        Task OnDeletedAsync(string inspector, int date, int time);
    }
}
