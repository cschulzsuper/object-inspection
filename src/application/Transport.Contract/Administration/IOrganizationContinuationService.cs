using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IOrganizationContinuationService
{
    ValueTask AddCreateInspectorContinuationForChiefInspectorAsync(Organization entity);

}