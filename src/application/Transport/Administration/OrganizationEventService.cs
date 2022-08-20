using Super.Paula.Application.Administration.Events;
using Super.Paula.Shared.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public class OrganizationEventService : IOrganizationEventService
{
    private readonly IEventStorage _eventStorage;
    private readonly ClaimsPrincipal _user;

    public OrganizationEventService(
        IEventStorage eventStorage,
        ClaimsPrincipal user)
    {
        _eventStorage = eventStorage;
        _user = user;
    }

    public async ValueTask CreateOrganizationCreationEventAsync(Organization entity)
    {
        var @event = new OrganizationCreationEvent(
            entity.UniqueName,
            entity.DisplayName,
            entity.Activated);

        await _eventStorage.AddAsync(@event, _user);
    }

    public async ValueTask CreateOrganizationUpdateEventAsync(Organization entity)
    {
        var @event = new OrganizationUpdateEvent(
            entity.UniqueName,
            entity.DisplayName,
            entity.Activated);

        await _eventStorage.AddAsync(@event, _user);
    }

    public async ValueTask CreateOrganizationDeletionEventAsync(string organization)
    {
        var @event = new OrganizationDeletionEvent(organization);

        await _eventStorage.AddAsync(@event, _user);
    }
}