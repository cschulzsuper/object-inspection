using ChristianSchulz.ObjectInspection.Application.Communication.Responses;
using System;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Communication;

internal sealed class NotificationCallbackHandler : INotificationCallbackHandler
{

    private readonly Receiver _receiver;

    public NotificationCallbackHandler(Receiver receiver)
    {
        _receiver = receiver;
    }

    public Task<IDisposable> OnCreationAsync(Func<NotificationResponse, Task> handler)
        => _receiver.OnAsync("NotificationCreation", handler);

    public Task<IDisposable> OnDeletionAsync(Func<string, int, int, Task> handler)
        => _receiver.OnAsync("NotificationDeletion", handler);
}