using Super.Paula.Application.Communication.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public interface INotificationCallbackHandler
    {
        Task<IDisposable> OnCreationAsync(Func<NotificationResponse, Task> handler);
        Task<IDisposable> OnDeletionAsync(Func<string, int, int, Task> handler);
    }
}