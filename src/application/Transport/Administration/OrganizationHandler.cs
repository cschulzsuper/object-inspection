using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class OrganizationHandler : IOrganizationHandler
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly IIdentityInspectorManager _identityInspectorManager;
        private readonly IOrganizationEventService _organizationEventService;

        public OrganizationHandler(
            IOrganizationManager organizationManager,
            IIdentityInspectorManager identityInspectorManager,
            IOrganizationEventService organizationEventService)
        {
            _organizationManager = organizationManager;
            _identityInspectorManager = identityInspectorManager;
            _organizationEventService = organizationEventService;
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

            await _organizationEventService.CreateOrganizationCreationEventAsync(entity);

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

            await _organizationEventService.CreateOrganizationDeletionEventAsync(organization);
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
                await _organizationEventService.CreateOrganizationUpdateEventAsync(entity);
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
                await _organizationEventService.CreateOrganizationUpdateEventAsync(entity);
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
                await _organizationEventService.CreateOrganizationUpdateEventAsync(entity);
            }
        }
    }
}