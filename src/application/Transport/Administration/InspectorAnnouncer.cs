using Super.Paula.Application.Administration.Responses;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class InspectorAnnouncer : IInspectorAnnouncer
    {
        private Func<string, InspectorBusinessObjectResponse, Task>? _onBusinessObjectCreationHandler;
        private Func<string, InspectorBusinessObjectResponse, Task>? _onBusinessObjectUpdateHandler;
        private Func<string, string, Task>? _onBusinessObjectDeletionHandler;

        public Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        {
            _onBusinessObjectCreationHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
        {
            _onBusinessObjectUpdateHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler)
        {
            _onBusinessObjectDeletionHandler = handler;
            return Task.FromResult<IDisposable>(null!);
        }

        public async Task AnnounceBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response)
        {
            var onBusinessObjectCreationTask = _onBusinessObjectCreationHandler?.Invoke(inspector, response);
            if (onBusinessObjectCreationTask != null) await onBusinessObjectCreationTask;
        }

        public async Task AnnounceBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response)
        {
            var onBusinessObjectUpdateTask = _onBusinessObjectUpdateHandler?.Invoke(inspector, response);
            if (onBusinessObjectUpdateTask != null) await onBusinessObjectUpdateTask;
        }

        public async Task AnnounceBusinessObjectDeletionAsync(string inspector, string businessObject)
        {
            var onBusinessObjectDeletionTask = _onBusinessObjectDeletionHandler?.Invoke(inspector, businessObject);
            if (onBusinessObjectDeletionTask != null) await onBusinessObjectDeletionTask;
        }
    }
}