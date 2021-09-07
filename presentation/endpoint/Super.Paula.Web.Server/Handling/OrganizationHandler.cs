using Super.Paula.Aggregates.Administration;
using Super.Paula.Management;
using Super.Paula.Management.Contract;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Server.Handling
{
    public class OrganizationHandler : IOrganizationHandler
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly IInspectorHandler _inspectorHandler;

        public OrganizationHandler(
            IOrganizationManager organizationManager,
            IInspectorHandler inspectorHandler)
        {
            _organizationManager = organizationManager;
            _inspectorHandler = inspectorHandler;
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
            var entitiy = await _organizationManager.GetAsync(organization);
            await _organizationManager.DeleteAsync(entitiy);
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

            var refreshOrganization =
                entity.Activated != request.Activated ||
                entity.DisplayName != request.DisplayName;

            entity.ChiefInspector = request.ChiefInspector;
            entity.DisplayName = request.DisplayName;
            entity.UniqueName = request.UniqueName;
            entity.Activated = request.Activated;

            await _organizationManager.UpdateAsync(entity);

            if (refreshOrganization)
            {
                await _inspectorHandler.RefreshOrganizationAsync(
                    entity.UniqueName,
                    new RefreshOrganizationRequest
                    {
                        DisplayName = entity.DisplayName,
                        Activated = entity.Activated
                    });
            }
        }

        public async ValueTask ActivateAsync(string organization)
        {
            var entity = await _organizationManager.GetAsync(organization);

            var refreshOrganization = entity.Activated != true;

            entity.Activated = true;

            await _organizationManager.UpdateAsync(entity);

            if (refreshOrganization)
            {
                await _inspectorHandler.RefreshOrganizationAsync(
                    entity.UniqueName,
                    new RefreshOrganizationRequest
                    {
                        DisplayName = entity.DisplayName,
                        Activated = entity.Activated
                    });
            }
        }

        public async ValueTask DeactivateAsync(string organization)

        {
            var entity = await _organizationManager.GetAsync(organization);

            var refreshOrganization = entity.Activated != false;

            entity.Activated = false;

            await _organizationManager.UpdateAsync(entity);

            if (refreshOrganization)
            {
                await _inspectorHandler.RefreshOrganizationAsync(
                    entity.UniqueName,
                    new RefreshOrganizationRequest
                    {
                        DisplayName = entity.DisplayName,
                        Activated = entity.Activated
                    });
            }
        }
    }
}