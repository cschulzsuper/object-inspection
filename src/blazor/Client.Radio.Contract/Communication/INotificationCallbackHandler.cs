using ChristianSchulz.ObjectInspection.Application.Communication.Responses;
using System;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Communication;

public interface INotificationCallbackHandler
{
    Task<IDisposable> OnCreationAsync(Func<NotificationResponse, Task> handler);
    Task<IDisposable> OnDeletionAsync(Func<string, int, int, Task> handler);
}