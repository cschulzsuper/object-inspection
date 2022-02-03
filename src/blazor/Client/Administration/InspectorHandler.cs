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
using Super.Paula.Client.Authentication;
using Super.Paula.Client.ErrorHandling;
using Super.Paula.Client.Streaming;
using Super.Paula.Environment;

namespace Super.Paula.Client.Administration
{
    public sealed class InspectorHandler : IInspectorHandler, IDisposable
    {
        private readonly IInspectorHandler _inspectorHandler;

        private readonly SemaphoreSlim _currentInspectorResponseCacheSemaphore;
        private InspectorResponse? _currentInspectorResponseCache;

        private readonly AuthenticationStateManager _authenticationStateManager;
        private readonly AppAuthentication _appAuthentication;

        public InspectorHandler(
            IInspectorHandler inspectorHandler,
            AuthenticationStateManager authenticationStateManager,
            AppAuthentication appAuthentication)
        {

            _authenticationStateManager = authenticationStateManager;
            _authenticationStateManager.AuthenticationStateChanged += AuthenticationStateChanged;

            _appAuthentication = appAuthentication;

            _inspectorHandler = inspectorHandler;
            _inspectorHandler.OnBusinessObjectCreationAsync(InternalOnBusinessObjectCreationAsync);
            _inspectorHandler.OnBusinessObjectUpdateAsync(InternalOnBusinessObjectUpdateAsync);
            _inspectorHandler.OnBusinessObjectDeletionAsync(InternalOnBusinessObjectDeletionAsync);

            _currentInspectorResponseCacheSemaphore = new SemaphoreSlim(1, 1);
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
                    await _currentInspectorResponseCacheSemaphore.WaitAsync();
                    _currentInspectorResponseCache = null;
                }
                finally
                {
                    _currentInspectorResponseCacheSemaphore.Release();
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
            if (_appAuthentication.Inspector != inspector)
            {
                await _inspectorHandler.GetAsync(inspector);
            }

            try
            {
                await _currentInspectorResponseCacheSemaphore.WaitAsync();

                if (_currentInspectorResponseCache == null ||
                    _currentInspectorResponseCache.UniqueName != inspector)
                {
                    _currentInspectorResponseCache = await _inspectorHandler.GetAsync(inspector);
                }
            }
            finally
            {
                _currentInspectorResponseCacheSemaphore.Release();
            }

            return _currentInspectorResponseCache;
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
            if (_appAuthentication.Inspector != inspector)
            {
                return;
            }


            try
            {
                await _currentInspectorResponseCacheSemaphore.WaitAsync();

                if (_currentInspectorResponseCache?.UniqueName == inspector)
                {
                    var inspectorBusinessObject = _currentInspectorResponseCache.BusinessObjects
                        .Single(x => x.UniqueName == businessObject);

                    _currentInspectorResponseCache.BusinessObjects.Remove(inspectorBusinessObject);
                }
            }
            finally
            {
                _currentInspectorResponseCacheSemaphore.Release();
            }
        }

        private async Task InternalOnBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse businessObject)
        {
            if (_appAuthentication.Inspector != inspector)
            {
                return;
            }

            try
            {
                await _currentInspectorResponseCacheSemaphore.WaitAsync();

                if (_currentInspectorResponseCache?.UniqueName == inspector)
                {
                    var inspectorBusinessObject = _currentInspectorResponseCache.BusinessObjects
                        .Single(x => x.UniqueName == businessObject.UniqueName);

                    _currentInspectorResponseCache.BusinessObjects.Remove(inspectorBusinessObject);
                    _currentInspectorResponseCache.BusinessObjects.Add(businessObject);
                }
            }
            finally
            {
                _currentInspectorResponseCacheSemaphore.Release();
            }
        }

        private async Task InternalOnBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse businessObject)
        {
            if (_appAuthentication.Inspector != inspector)
            {
                return;
            }

            try
            {
                await _currentInspectorResponseCacheSemaphore.WaitAsync();

                if (_currentInspectorResponseCache?.UniqueName == inspector)
                {
                    _currentInspectorResponseCache.BusinessObjects.Add(businessObject);
                }
            }
            finally
            {
                _currentInspectorResponseCacheSemaphore.Release();
            }
        }
    }
}
