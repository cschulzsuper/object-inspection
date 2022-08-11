using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorCallbackHandler
    {
        Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
        Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
        Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler);
    }
}