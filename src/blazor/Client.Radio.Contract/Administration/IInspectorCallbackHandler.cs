using Super.Paula.Application.Administration.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration;

public interface IInspectorCallbackHandler
{
    Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
    Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
    Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler);
}