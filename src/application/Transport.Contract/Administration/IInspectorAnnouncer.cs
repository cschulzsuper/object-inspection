using Super.Paula.Application.Administration.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorAnnouncer
    {
        Task AnnounceBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response);
        Task AnnounceBusinessObjectDeletionAsync(string inspector, string businessObject);
        Task AnnounceBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response);
        Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
        Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler);
        Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler);
    }
}