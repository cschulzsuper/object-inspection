using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Administration.Continuation;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class InspectorContinuationHandler : IInspectorContinuationHandler
{
    public async Task HandleAsync(ContinuationHandlerContext context, CreateInspectorContinuation continuation)
    {
        var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();
        var inspectorContinuationService = context.Services.GetRequiredService<IInspectorContinuationService>();

        var entity = new Inspector
        {
            OrganizationActivated = continuation.OrganizationActivated,
            Activated = continuation.Activated,
            Identity = continuation.Identity,
            Organization = continuation.Organization,
            UniqueName = continuation.UniqueName,
            OrganizationDisplayName = continuation.OrganizationDisplayName
        };

        await inspectorManager.InsertAsync(entity);
        await inspectorContinuationService.AddCreateIdentityInspectorContinuationAsync(entity);
    }
}