using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IOrganizationEventService
{
    ValueTask CreateOrganizationCreationEventAsync(Organization entity);

    ValueTask CreateOrganizationUpdateEventAsync(Organization entity);

    ValueTask CreateOrganizationDeletionEventAsync(string organization);
}