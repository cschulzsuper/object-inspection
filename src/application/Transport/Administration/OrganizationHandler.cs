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
        private readonly IEventBus _eventBus;
        private readonly ClaimsPrincipal _user;

        public OrganizationHandler(
            IOrganizationManager organizationManager,
            IEventBus eventBus,
            ClaimsPrincipal user)
        {
            _organizationManager = organizationManager;
            _eventBus = eventBus;
            _user = user;
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

            await PublishOrganizationAsync(entity);

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

                await PublishOrganizationAsync(entity);
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

                await PublishOrganizationAsync(entity);
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

                await PublishOrganizationAsync(entity);
            }
        }

        private async ValueTask PublishOrganizationAsync(Organization entity)
        {
            var @event = new OrganizationEvent(
                entity.UniqueName,
                entity.DisplayName,
                entity.Activated);

            await _eventBus.PublishAsync(@event, _user);
        }
    }
}