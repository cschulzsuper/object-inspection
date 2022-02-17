using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Runtime;
using Super.Paula.Authorization;
using Super.Paula.Environment;

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

        public AccountHandler(
            IInspectorManager inspectorManager,
            IIdentityInspectorManager identityInspectorManager,
            IOrganizationManager organizationManager,
            AppSettings appSettings,
            ITokenAuthorizationFilter tokenAuthorizatioFilter,
            IConnectionManager connectionManager,
            ClaimsPrincipal user)
        {
            _inspectorManager = inspectorManager;
            _identityInspectorManager = identityInspectorManager;
            _organizationManager = organizationManager;
            _appSettings = appSettings;
            _tokenAuthorizatioFilter = tokenAuthorizatioFilter;
            _connectionManager = connectionManager;
            _user = user;
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            if (_appSettings.MaintainerOrganization != request.UniqueName)
            {
                throw new TransportException($"Only the maintainer organization can be registered");
            }

            if (_appSettings.Maintainer != request.ChiefInspector)
            {
                throw new TransportException($"Only the maintainer can register with an organization");
            }

            await _organizationManager.InsertAsync(new Organization
            {
                ChiefInspector = request.ChiefInspector,
                UniqueName = request.UniqueName,
                DisplayName = request.DisplayName,
                Activated = true
            });

            await _inspectorManager.InsertAsync(new Inspector
            {
                Identity = request.Identity,
                Organization = request.UniqueName,
                OrganizationActivated = true,
                OrganizationDisplayName = request.DisplayName,
                UniqueName = request.ChiefInspector,
                Activated = true
            });
        }

        public ValueTask<string> SignInInspectorAsync(SignInInspectorRequest request)
        {
            var identityInspector = _identityInspectorManager
                .GetIdentityBasedQueryable(_user.GetIdentity())
                .Single(x =>
                    x.Activated &&
                    x.Inspector == request.Inspector &&
                    x.Organization == request.Organization);

            var token = _user.ToToken();

            token.Inspector = identityInspector.Inspector;
            token.Organization = identityInspector.Organization;

            _connectionManager.Trace(
                $"{token.Organization}:{token.Inspector}",
                token.Proof!);

            _tokenAuthorizatioFilter.Apply(token);

            return ValueTask.FromResult(token.ToBase64String());
        }

        public ValueTask<string> StartImpersonationAsync(StartImpersonationRequest request)
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == request.UniqueName &&
                    x.Organization == request.Organization);

            var token = _user.ToToken();

            token.Proof = _user.GetProof();
            token.Inspector = inspector.UniqueName;
            token.Organization = inspector.Organization;
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
    }
}