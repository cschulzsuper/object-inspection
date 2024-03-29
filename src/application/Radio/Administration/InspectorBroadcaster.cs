﻿using Microsoft.AspNetCore.SignalR;
using ChristianSchulz.ObjectInspection.Application.Administration.Responses;
using ChristianSchulz.ObjectInspection.Shared.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public sealed class InspectorBroadcaster : IInspectorBroadcaster, IDisposable
{
    private readonly IHubContext _radioHubContext;
    private readonly ClaimsPrincipal _claimsPrincipal;

    private bool _disposed;

    public InspectorBroadcaster(HubContextResolver hubContextResolver, ClaimsPrincipal claimsPrincipal)
    {
        _radioHubContext = hubContextResolver.GetHubContext("Radio");
        _claimsPrincipal = claimsPrincipal;
    }

    public void Dispose()
    {
        _disposed = true;
    }

    public async Task SendInspectorBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response)
    {
        if (_disposed)
        {
            return;
        }

        var userId = $"{_claimsPrincipal.Claims.GetOrganization()}:{inspector}";
        await _radioHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectCreation", inspector, response);
    }

    public async Task SendInspectorBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response)
    {
        if (_disposed)
        {
            return;
        }

        var userId = $"{_claimsPrincipal.Claims.GetOrganization()}:{inspector}";
        await _radioHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectUpdate", inspector, response);
    }

    public async Task SendInspectorBusinessObjectDeletionAsync(string inspector, string businessObject)
    {
        if (_disposed)
        {
            return;
        }

        var userId = $"{_claimsPrincipal.Claims.GetOrganization()}:{inspector}";
        await _radioHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectDeletion", inspector, businessObject);
    }
}