using Microsoft.AspNetCore.Components.Authorization;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Super.Paula.Shared.Security;

namespace Super.Paula.Client.Administration;

public sealed class ExtendedInspectorRequestHandler : IInspectorRequestHandler, IDisposable
{
    private readonly IInspectorRequestHandler _inspectorRequestHandler;
    private readonly IInspectorCallbackHandler _inspectorCallbackHandler;

    private readonly SemaphoreSlim _currentInspectorResponseCacheSemaphore;
    private InspectorResponse? _currentInspectorResponseCache;

    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public ExtendedInspectorRequestHandler(
        IInspectorRequestHandler inspectorRequestHandler,
        IInspectorCallbackHandler inspectorCallbackHandler,
        AuthenticationStateProvider authenticationStateProvider)
    {

        _authenticationStateProvider = authenticationStateProvider;
        _authenticationStateProvider.AuthenticationStateChanged += AuthenticationStateChanged;

        _inspectorRequestHandler = inspectorRequestHandler;
        _inspectorCallbackHandler = inspectorCallbackHandler;
        _inspectorCallbackHandler.OnBusinessObjectCreationAsync(InternalOnBusinessObjectCreationAsync);
        _inspectorCallbackHandler.OnBusinessObjectUpdateAsync(InternalOnBusinessObjectUpdateAsync);
        _inspectorCallbackHandler.OnBusinessObjectDeletionAsync(InternalOnBusinessObjectDeletionAsync);

        _currentInspectorResponseCacheSemaphore = new(1);
    }

    public void Dispose()
    {
        _authenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateChanged;

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

    public ValueTask<ActivateInspectorResponse> ActivateAsync(string inspector, string etag)
        => _inspectorRequestHandler.ActivateAsync(inspector, etag);

    public ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
        => _inspectorRequestHandler.CreateAsync(request);

    public ValueTask<DeactivateInspectorResponse> DeactivateAsync(string inspector, string etag)
        => _inspectorRequestHandler.DeactivateAsync(inspector, etag);

    public ValueTask DeleteAsync(string inspector, string etag)
        => _inspectorRequestHandler.DeleteAsync(inspector, etag);

    public IAsyncEnumerable<InspectorResponse> GetAll()
         => _inspectorRequestHandler.GetAll();

    public IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
        => _inspectorRequestHandler.GetAllForOrganization(organization);

    public async ValueTask<InspectorResponse> GetAsync(string inspector)
    {
        var user = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;

        if (!user.Claims.HasInspector(inspector))
        {
            return await _inspectorRequestHandler.GetAsync(inspector);
        }

        try
        {
            await _currentInspectorResponseCacheSemaphore.WaitAsync();

            _currentInspectorResponseCache ??= await _inspectorRequestHandler.GetAsync(inspector);

            return _currentInspectorResponseCache;
        }
        finally
        {
            _currentInspectorResponseCacheSemaphore.Release();
        }
    }

    public async ValueTask<InspectorResponse> GetCurrentAsync()
    {
        try
        {
            await _currentInspectorResponseCacheSemaphore.WaitAsync();

            _currentInspectorResponseCache ??= await _inspectorRequestHandler.GetCurrentAsync();

            return _currentInspectorResponseCache;
        }
        finally
        {
            _currentInspectorResponseCacheSemaphore.Release();
        }
    }

    public ValueTask ReplaceAsync(string inspector, InspectorRequest request)
        => _inspectorRequestHandler.ReplaceAsync(inspector, request);

    private async Task InternalOnBusinessObjectDeletionAsync(string inspector, string businessObject)
    {
        var user = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;

        if (!user.Claims.HasInspector(inspector))
        {
            return;
        }

        try
        {
            await _currentInspectorResponseCacheSemaphore.WaitAsync();

            if (_currentInspectorResponseCache != null)
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
        var user = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;

        if (!user.Claims.HasInspector(inspector))
        {
            return;
        }

        try
        {
            await _currentInspectorResponseCacheSemaphore.WaitAsync();

            if (_currentInspectorResponseCache != null)
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
        var user = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;

        if (!user.Claims.HasInspector(inspector))
        {
            return;
        }

        try
        {
            await _currentInspectorResponseCacheSemaphore.WaitAsync();

            if (_currentInspectorResponseCache != null)
            {
                _currentInspectorResponseCache.BusinessObjects.Add(businessObject);
            }
        }
        finally
        {
            _currentInspectorResponseCacheSemaphore.Release();
        }
    }

    public IAsyncEnumerable<IdentityInspectorResponse> GetAllForIdentity(string identity)
        => _inspectorRequestHandler.GetAllForIdentity(identity);
}