using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Orchestration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class OrganizationHandler : IOrganizationHandler
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly IIdentityInspectorManager _identityInspectorManager;
        private readonly IEventBus _eventBus;

        public OrganizationHandler(
            IOrganizationManager organizationManager,
            IIdentityInspectorManager identityInspectorManager,
            IEventBus eventBus)
        {
            _organizationManager = organizationManager;
            _identityInspectorManager = identityInspectorManager;
            _eventBus = eventBus;
        }

        public async ValueTask<OrganizationResponse> CreateAsync(OrganizationRequest request)
        {
            var entity = new Organization
            {
                ChiefInspector = request.ChiefInspector,
                DisplayName = request.DisplayName,
                UniqueName = request.UniqueName,
                Activated = request.Activated
            };

            await _organizationManager.InsertAsync(entity);

            await PublishOrganizationCreationAsync(entity);

            return new OrganizationResponse
            {
                ChiefInspector = entity.ChiefInspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated
            };
        }

        public async ValueTask DeleteAsync(string organization)
        {
            var entity = await _organizationManager.GetAsync(organization);
            
            await _organizationManager.DeleteAsync(entity);

            var identityInspectors = _identityInspectorManager.GetQueryable()
                .Where(x => x.Organization == organization)
                .ToList();

            foreach (var identityInspector in identityInspectors)
            {
                await _identityInspectorManager.DeleteAsync(identityInspector);
            }

            await PublishOrganizationDeletionAsync(organization);
        }

        public IAsyncEnumerable<OrganizationResponse> GetAll()
            => _organizationManager
                .GetAsyncEnumerable(query => query
                    .Select(entity => new OrganizationResponse
                    {
                        ChiefInspector = entity.ChiefInspector,
                        DisplayName = entity.DisplayName,
                        UniqueName = entity.UniqueName,
                        Activated = entity.Activated
                    }));

        public async ValueTask<OrganizationResponse> GetAsync(string organization)
        {
            var entity = await _organizationManager.GetAsync(organization);

            return new OrganizationResponse
            {
                ChiefInspector = entity.ChiefInspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated
            };
        }

        public async ValueTask ReplaceAsync(string organization, OrganizationRequest request)
        {
            var entity = await _organizationManager.GetAsync(organization);

            var required =
                entity.Activated != request.Activated ||
                entity.DisplayName != request.DisplayName ||
                entity.ChiefInspector != request.ChiefInspector ||
                entity.UniqueName != request.UniqueName;

            if (required)
            {
                entity.Activated = request.Activated;
                entity.DisplayName = request.DisplayName;
                entity.ChiefInspector = request.ChiefInspector;
                entity.UniqueName = request.UniqueName;

                await _organizationManager.UpdateAsync(entity);

                await PublishOrganizationUpdateAsync(entity);
            }
        }

        public async ValueTask ActivateAsync(string organization)
        {
            var entity = await _organizationManager.GetAsync(organization);

            var required = !entity.Activated;
            if (required)
            {
                entity.Activated = true;

                await _organizationManager.UpdateAsync(entity);

                await PublishOrganizationUpdateAsync(entity);
            }
        }

        public async ValueTask DeactivateAsync(string organization)
        {
            var entity = await _organizationManager.GetAsync(organization);

            var required = entity.Activated;
            if (required)
            {
                entity.Activated = false;

                await _organizationManager.UpdateAsync(entity);

                await PublishOrganizationUpdateAsync(entity);
            }
        }

        private async ValueTask PublishOrganizationCreationAsync(Organization entity)
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

            await _eventBus.PublishAsync(@event, user);
        }

        private async ValueTask PublishOrganizationUpdateAsync(Organization entity)
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

            await _eventBus.PublishAsync(@event, user);
        }

        private async ValueTask PublishOrganizationDeletionAsync(string organization)
        {
            var @event = new OrganizationDeletionEvent(organization);

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim("Organization", organization)
                    }));

            await _eventBus.PublishAsync(@event, user);
        }
    }
}