using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Client.Authentication;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Client.Streaming;
using Super.Paula.Environment;

namespace Super.Paula.Client.Administration
{
    public sealed class InspectorHandler : IInspectorHandler, IDisposable
    {
        private readonly IInspectorHandler _inspectorHandler;

        private readonly SemaphoreSlim _inspectorResponseCacheSemaphore;
        private InspectorResponse? _inspectorResponseCache;

        private readonly AuthenticationStateManager _authenticationStateManager;

        public InspectorHandler(
            IInspectorHandler inspectorHandler,
            AuthenticationStateManager authenticationStateManager)
        {

            _authenticationStateManager = authenticationStateManager;
            _authenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _inspectorHandler = inspectorHandler;
            _inspectorHandler.OnBusinessObjectCreationAsync(InternalOnBusinessObjectCreationAsync);
            _inspectorHandler.OnBusinessObjectUpdateAsync(InternalOnBusinessObjectUpdateAsync);
            _inspectorHandler.OnBusinessObjectDeletionAsync(InternalOnBusinessObjectDeletionAsync);

            _inspectorResponseCacheSemaphore = new SemaphoreSlim(1, 1);
        }

        public void Dispose()
        {
            _authenticationStateManager.AuthenticationStateChanged -= AuthenticationStateChanged;

            GC.SuppressFinalize(this);
        }

        private void AuthenticationStateChanged(Task<AuthenticationState> task)
            => task.ContinueWith(async _ =>
            {
                try
                {
                    await _inspectorResponseCacheSemaphore.WaitAsync();
                    _inspectorResponseCache = null;
                }
                finally
                {
                    _inspectorResponseCacheSemaphore.Release();
                }
            });

        public ValueTask ActivateAsync(string inspector)
            => _inspectorHandler.ActivateAsync(inspector);

        public ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
            => _inspectorHandler.CreateAsync(request);

        public ValueTask DeactivateAsync(string inspector)
            => _inspectorHandler.DeactivateAsync(inspector);

        public ValueTask DeleteAsync(string inspector)
            => _inspectorHandler.DeleteAsync(inspector);

        public IAsyncEnumerable<InspectorResponse> GetAll()
             => _inspectorHandler.GetAll();

        public IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
            => _inspectorHandler.GetAllForOrganization(organization);

        public async ValueTask<InspectorResponse> GetAsync(string inspector)
        {
            try
            {
                await _inspectorResponseCacheSemaphore.WaitAsync();

                if (_inspectorResponseCache == null ||
                    _inspectorResponseCache.UniqueName != inspector)
                { 
                    _inspectorResponseCache = await _inspectorHandler.GetAsync(inspector);
                }
            }
            finally
            {
                _inspectorResponseCacheSemaphore.Release();
            }

            return _inspectorResponseCache;
        }

        public Task<IDisposable> OnBusinessObjectCreationAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
            => _inspectorHandler.OnBusinessObjectCreationAsync(handler);

        public Task<IDisposable> OnBusinessObjectDeletionAsync(Func<string, string, Task> handler)
            => _inspectorHandler.OnBusinessObjectDeletionAsync(handler);

        public Task<IDisposable> OnBusinessObjectUpdateAsync(Func<string, InspectorBusinessObjectResponse, Task> handler)
            => _inspectorHandler.OnBusinessObjectUpdateAsync(handler);

        public ValueTask ReplaceAsync(string inspector, InspectorRequest request)
            => _inspectorHandler.ReplaceAsync(inspector, request);

        private async Task InternalOnBusinessObjectDeletionAsync(string inspector, string businessObject)
        {
            try
            {
                await _inspectorResponseCacheSemaphore.WaitAsync();

                if (_inspectorResponseCache?.UniqueName == inspector)
                {
                    var inspectorBusinessObject = _inspectorResponseCache.BusinessObjects
                        .Single(x => x.UniqueName == businessObject);

                    _inspectorResponseCache.BusinessObjects.Remove(inspectorBusinessObject);
                }
            }
            finally
            {
                _inspectorResponseCacheSemaphore.Release();
            }
        }

        private async Task InternalOnBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse businessObject)
        {
            try
            {
                await _inspectorResponseCacheSemaphore.WaitAsync();

                if (_inspectorResponseCache?.UniqueName == inspector)
                {
                    var inspectorBusinessObject = _inspectorResponseCache.BusinessObjects
                        .Single(x => x.UniqueName == businessObject.UniqueName);

                    _inspectorResponseCache.BusinessObjects.Remove(inspectorBusinessObject);
                    _inspectorResponseCache.BusinessObjects.Add(businessObject);
                }
            }
            finally
            {
                _inspectorResponseCacheSemaphore.Release();
            }
        }

        private async Task InternalOnBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse businessObject)
        {
            try
            {
                await _inspectorResponseCacheSemaphore.WaitAsync();

                if (_inspectorResponseCache?.UniqueName == inspector)
                {
                    _inspectorResponseCache.BusinessObjects.Add(businessObject);
                }
            }
            finally
            {
                _inspectorResponseCacheSemaphore.Release();
            }
        }
    }
}
