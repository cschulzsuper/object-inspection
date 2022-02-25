using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IOrganizationEventService
    {
        ValueTask CreateOrganizationCreationEventAsync(Organization entity);

        ValueTask CreateOrganizationUpdateEventAsync(Organization entity);

        ValueTask CreateOrganizationDeletionEventAsync(string organization);
    }
}