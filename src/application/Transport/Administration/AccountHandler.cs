using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Operation;
using Super.Paula.Application.Orchestration;
using Super.Paula.Authorization;
using Super.Paula.Environment;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IInspectorManager _inspectorManager;
        private readonly IIdentityInspectorManager _identityInspectorManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly AppSettings _appSettings;
        private readonly ITokenAuthorizationFilter _tokenAuthorizatioFilter;
        private readonly IConnectionManager _connectionManager;
        private readonly ClaimsPrincipal _user;
        private readonly IEventBus _eventBus;

        public AccountHandler(
            IInspectorManager inspectorManager,
            IIdentityInspectorManager identityInspectorManager,
            IOrganizationManager organizationManager,
            AppSettings appSettings,
            ITokenAuthorizationFilter tokenAuthorizatioFilter,
            IConnectionManager connectionManager,
            ClaimsPrincipal user,
            IEventBus eventBus)
        {
            _inspectorManager = inspectorManager;
            _identityInspectorManager = identityInspectorManager;
            _organizationManager = organizationManager;
            _appSettings = appSettings;
            _tokenAuthorizatioFilter = tokenAuthorizatioFilter;
            _connectionManager = connectionManager;
            _user = user;
            _eventBus = eventBus;
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            if (_appSettings.MaintainerIdentity != _user.GetIdentity())
            {
                throw new TransportException($"Only the maintainer organization can be registered.");
            }

            var organization = new Organization
            {
                ChiefInspector = request.UniqueName,
                UniqueName = request.UniqueName,
                DisplayName = request.DisplayName,
                Activated = true
            };

            await _organizationManager.InsertAsync(organization);

            await PublishOrganizationCreationAsync(organization);
        }

        public async ValueTask RegisterChiefInspectorAsync(string organization, RegisterChiefInspectorRequest request)
        {
            if (_appSettings.MaintainerIdentity != _user.GetIdentity())
            { 
                throw new TransportException($"Only the maintainer can register the chief inspectors.");
            }

            var organizationEntity = await _organizationManager.GetAsync(organization);

            organizationEntity.ChiefInspector =  request.Inspector;

            await _inspectorManager.InsertAsync(new Inspector
            {
                Identity = request.Identity,
                Organization = organization,
                OrganizationActivated = true,
                OrganizationDisplayName = organizationEntity.DisplayName,
                UniqueName = request.Inspector,
                Activated = true
            });

            await _identityInspectorManager.InsertAsync(new IdentityInspector
            {
                Activated = true,
                Inspector = request.Inspector,
                Organization = organization,
                UniqueName = request.Identity
            });

            await _organizationManager.UpdateAsync(organizationEntity);
        }

        public ValueTask<string> SignInInspectorAsync(string organization, string inspector)
        {
            var identityInspector = _identityInspectorManager
                .GetIdentityBasedQueryable(_user.GetIdentity())
                .Single(x =>
                    x.Activated &&
                    x.Inspector == inspector &&
                    x.Organization == organization);

            var token = _user.ToToken();

            token.Inspector = identityInspector.Inspector;
            token.Organization = identityInspector.Organization;

            _connectionManager.Trace(
                $"{token.Organization}:{token.Inspector}",
                token.Proof!);

            _tokenAuthorizatioFilter.Apply(token);

            return ValueTask.FromResult(token.ToBase64String());
        }

        public ValueTask<string> StartImpersonationAsync(string organization, string inspector)
        {
            var entity = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == inspector &&
                    x.Organization == organization);

            var token = _user.ToToken();

            token.Proof = _user.GetProof();
            token.Inspector = entity.UniqueName;
            token.Organization = entity.Organization;
            token.ImpersonatorInspector = _user.GetInspector();
            token.ImpersonatorOrganization = _user.GetOrganization();

            _tokenAuthorizatioFilter.Apply(token);

            return ValueTask.FromResult(token.ToBase64String());
        }

        public ValueTask<string> StopImpersonationAsync()
        {
            var inspector = _inspectorManager.GetQueryable()
               .Single(x =>
                   x.Activated &&
                   x.OrganizationActivated &&
                   x.UniqueName == _user.GetImpersonatorInspector() &&
                   x.Organization == _user.GetImpersonatorOrganization());

            var token = _user.ToToken();

            token.Inspector = inspector.UniqueName;
            token.Organization = inspector.Organization;
            token.ImpersonatorInspector = null;
            token.ImpersonatorOrganization = null;

            _tokenAuthorizatioFilter.Apply(token);

            return ValueTask.FromResult(token.ToBase64String());
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
    }
}