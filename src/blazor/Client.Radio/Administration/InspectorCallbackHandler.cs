using Super.Paula.Application.Administration.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration;

internal class InspectorCallbackHandler : IInspectorCallbackHandler
{
    private readonly Receiver _receiver;

    public InspectorCallbackHandler(Receiver receiver)
    {
        _receiver = receiver;
    }

    public Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        => _receiver.OnAsync("InspectorBusinessObjectCreation", handler);

    public Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        => _receiver.OnAsync("InspectorBusinessObjectUpdate", handler);

    public Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler)
        => _receiver.OnAsync("InspectorBusinessObjectDeletion", handler);
}