using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Environment;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Super.Paula.Application.Administration.Events;

namespace Super.Paula.Application.Administration
{
    public class InspectorHandler : 
        IInspectorHandler,
        IInspectorEventHandler
    {
        private readonly IInspectorManager _inspectorManager;
        private readonly IOrganizationProvider _organizationProvider;
        private readonly AppState _appState;

        public InspectorHandler(
            IInspectorManager inspectorManager,
            IOrganizationProvider organizationProvider,
            AppState appState) 
        {
            _inspectorManager = inspectorManager;
            _organizationProvider = organizationProvider;
            _appState = appState;
        }

        public async ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
        {
            var organization = await _organizationProvider.GetAsync(_appState.CurrentOrganization);

            var entity = new Inspector
            {
                Identity = request.UniqueName,
                UniqueName = request.UniqueName,
                Activated = request.Activated,
                Organization = organization.UniqueName,
                OrganizationActivated = organization.Activated,
                OrganizationDisplayName = organization.DisplayName
            };

            await _inspectorManager.InsertAsync(entity);

            return new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated
            };
        }

        public async ValueTask DeleteAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            await _inspectorManager.DeleteAsync(entity);
        }

        public IAsyncEnumerable<InspectorResponse> GetAll()
            => _inspectorManager
                .GetAsyncEnumerable(query => query
                .Select(entity => new InspectorResponse
                {
                    Identity = entity.Identity,
                    UniqueName = entity.UniqueName,
                    Activated = entity.Activated
                }));

        public async ValueTask<InspectorResponse> GetAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);
         
            return new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated
            };
        }

        public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Identity = request.Identity;
            entity.UniqueName = request.UniqueName;
            entity.Activated = request.Activated;

            await _inspectorManager.UpdateAsync(entity);
        }

        public async ValueTask ActivateAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Activated = true;

            await _inspectorManager.UpdateAsync(entity);
        }

        public async ValueTask DeactivateAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.Activated = false;

            await _inspectorManager.UpdateAsync(entity);
        }

        public IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
        {
            return _inspectorManager
               .GetAsyncEnumerable(query => query
                   .Where(x => x.Organization == organization)
                   .Select(entity => new InspectorResponse
                   {
                       Identity = entity.Identity,
                       UniqueName = entity.UniqueName,
                       Activated = entity.Activated
                   }));
        }

        public async ValueTask ProcessAsync(string organization, OrganizationEvent @event)
        {
            var inspectors = _inspectorManager.GetQueryable()
                .Where(x => x.Organization == organization)
                .ToList();

            foreach (var inspector in inspectors)
            {
                inspector.OrganizationActivated =  @event.Activated ?? inspector.OrganizationActivated;
                inspector.OrganizationDisplayName =  @event.DisplayName ?? inspector.OrganizationDisplayName;

                await _inspectorManager.UpdateAsync(inspector);
            }
        }
    }
}