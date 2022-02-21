using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Orchestration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class OrganizationEventService : IOrganizationEventService
    {
        private readonly IEventStorage _eventStorage;

        public OrganizationEventService(IEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

        public async ValueTask CreateOrganizationCreationEventAsync(Organization entity)
        {
            var @event = new OrganizationCreationEvent(
                entity.UniqueName,
                entity.DisplayName,
                entity.Activated);

            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim("Organization", entity.UniqueName)
                        }));

            await _eventStorage.AddAsync(@event, user);
        }

        public async ValueTask CreateOrganizationUpdateEventAsync(Organization entity)
        {
            var @event = new OrganizationUpdateEvent(
                entity.UniqueName,
                entity.DisplayName,
                entity.Activated);

            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim("Organization", entity.UniqueName)
                        }));

            await _eventStorage.AddAsync(@event, user);
        }

        public async ValueTask CreateOrganizationDeletionEventAsync(string organization)
        {
            var @event = new OrganizationDeletionEvent(organization);

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim("Organization", organization)
                    }));

            await _eventStorage.AddAsync(@event, user);
        }
    }
}