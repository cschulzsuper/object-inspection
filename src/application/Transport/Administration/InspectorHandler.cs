using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Environment;

namespace Super.Paula.Application.Administration
{
    internal class InspectorHandler : IInspectorHandler
    {
        private readonly IInspectorManager _inspectorManager;
        private readonly Lazy<IOrganizationHandler> _organizationHandler;
        private readonly AppState _appState;

        public InspectorHandler(
            IInspectorManager inspectorManager,
            Lazy<IOrganizationHandler> organizationHandler,
            AppState appState) 
        {
            _inspectorManager = inspectorManager;
            _organizationHandler = organizationHandler;
            _appState = appState;
        }

        public async ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
        {
            var organization = await _organizationHandler.Value.GetAsync(_appState.CurrentOrganization);

            var entity = new Inspector
            {
                MailAddress = request.MailAddress,
                Secret = request.Secret,
                UniqueName = request.UniqueName,
                Activated = request.Activated,
                Organization = organization.UniqueName,
                OrganizationActivated = organization.Activated,
                OrganizationDisplayName = organization.DisplayName,
                Proof = $"{Guid.NewGuid()}"
            };

            await _inspectorManager.InsertAsync(entity);

            return new InspectorResponse
            {
                MailAddress = entity.MailAddress,
                Secret = entity.Secret,
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
                    MailAddress = entity.MailAddress,
                    Secret = entity.Secret,
                    UniqueName = entity.UniqueName,
                    Activated = entity.Activated
                }));

        public async ValueTask<InspectorResponse> GetAsync(string inspector)
        {
            var entity = await _inspectorManager.GetAsync(inspector);
         
            return new InspectorResponse
            {
                MailAddress = entity.MailAddress,
                Secret = entity.Secret,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated
            };
        }

        public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
        {
            var entity = await _inspectorManager.GetAsync(inspector);

            entity.MailAddress = request.MailAddress;
            entity.Secret = request.Secret;
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
                       MailAddress = entity.MailAddress,
                       Secret = entity.Secret,
                       UniqueName = entity.UniqueName,
                       Activated = entity.Activated
                   }));
        }

        public async ValueTask RefreshOrganizationAsync(string organization, RefreshOrganizationRequest request)
        {
            var inspectors = _inspectorManager.GetQueryable()
                .Where(x => x.Organization == organization)
                .ToList();

            foreach(var inspector in inspectors)
            {
                inspector.OrganizationActivated = request.Activated;
                inspector.OrganizationDisplayName = request.DisplayName;

                await _inspectorManager.UpdateAsync(inspector);
            }
        }
    }
}