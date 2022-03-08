using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IOrganizationContinuationService
    {
        ValueTask AddCreateInspectorContinuationForChiefInspectorAsync(Organization entity);

    }
}