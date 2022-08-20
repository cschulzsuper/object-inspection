using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public interface IInspectorContinuationService
{
    ValueTask AddCreateIdentityInspectorContinuationAsync(Inspector entity);

    ValueTask AddActivateIdentityInspectorContinuationAsync(Inspector entity);

    ValueTask AddDeactivateIdentityInspectorContinuationAsync(Inspector entity);

    ValueTask AddDeleteIdentityInspectorContinuationAsync(Inspector entity);

    ValueTask AddDeleteIdentityInspectorContinuationAsync(string identity, string organization, string uniqueName);
}