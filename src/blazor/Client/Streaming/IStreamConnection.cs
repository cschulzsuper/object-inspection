using Super.Paula.Application.Communication.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Client.Streaming
{
    public interface IStreamConnection
    {
        Task<IDisposable> OnNotificationCreationAsync(Func<NotificationResponse, Task> handler);

        Task<IDisposable> OnNotificationDeletionAsync(Func<string, int, int, Task> handler);
    }
}
