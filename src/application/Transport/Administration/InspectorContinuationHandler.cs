using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration.Continuation;
using Super.Paula.Shared.Orchestration;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

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