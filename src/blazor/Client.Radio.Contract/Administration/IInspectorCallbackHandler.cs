using ChristianSchulz.ObjectInspection.Application.Administration.Responses;
using System;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Administration;

public interface IInspectorCallbackHandler
{
    Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
    Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
    Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler);
}