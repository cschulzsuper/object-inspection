﻿using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Administration.Continuation;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class IdentityInspectorContinuationHandler : IIdentityInspectorContinuationHandler
{
    public async Task HandleAsync(ContinuationHandlerContext context, CreateIdentityInspectorContinuation continuation)
    {
        var identityInspectorManager = context.Services.GetRequiredService<IIdentityInspectorManager>();

        var identityInspector = new IdentityInspector
        {
            UniqueName = continuation.UniqueName,
            Organization = continuation.Organization,
            Inspector = continuation.Inspector,
            Activated = continuation.Activated
        };

        await identityInspectorManager.InsertAsync(identityInspector);
    }

    public async Task HandleAsync(ContinuationHandlerContext context, ActivateIdentityInspectorContinuation continuation)
    {
        var identityInspectorManager = context.Services.GetRequiredService<IIdentityInspectorManager>();

        var identityInspector = await identityInspectorManager.GetAsync(
            continuation.UniqueName,
            continuation.Organization,
            continuation.Inspector);

        identityInspector.Activated = true;

        await identityInspectorManager.UpdateAsync(identityInspector);
    }

    public async Task HandleAsync(ContinuationHandlerContext context, DeleteIdentityInspectorContinuation continuation)
    {
        var identityInspectorManager = context.Services.GetRequiredService<IIdentityInspectorManager>();

        var identityInspector = await identityInspectorManager.GetAsync(
            continuation.UniqueName,
            continuation.Organization,
            continuation.Inspector);

        await identityInspectorManager.DeleteAsync(identityInspector);
    }

    public async Task HandleAsync(ContinuationHandlerContext context, DeactivateIdentityInspectorContinuation continuation)
    {
        var identityInspectorManager = context.Services.GetRequiredService<IIdentityInspectorManager>();

        var identityInspector = await identityInspectorManager.GetAsync(
            continuation.UniqueName,
            continuation.Organization,
            continuation.Inspector);

        identityInspector.Activated = false;

        await identityInspectorManager.UpdateAsync(identityInspector);
    }
}