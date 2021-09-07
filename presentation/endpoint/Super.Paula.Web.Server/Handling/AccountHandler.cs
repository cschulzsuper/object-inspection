using System.Text;
using Super.Paula.Aggregates.Administration;
using Super.Paula.Environment;
using Super.Paula.Management.Contract;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Server.Handling
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IInspectorManager _inspectorManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly AppState _appState;
        private readonly AppSettings _appSettings;
        private readonly AppAuthentication _appAuthentication;

        public AccountHandler(
            IInspectorManager inspectorManager,
            IOrganizationManager organizationManager,
            AppState appState,
            AppSettings appSettings,
            AppAuthentication appAuthentication)
        {
            _inspectorManager = inspectorManager;
            _organizationManager = organizationManager;
            _appState = appState;
            _appSettings = appSettings;
            _appAuthentication = appAuthentication;
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == _appState.CurrentInspector &&
                    x.Organization == _appState.CurrentOrganization &&
                    x.Secret == request.OldSecret);

            inspector.Secret = request.NewSecret;

            await _inspectorManager.UpdateAsync(inspector);
        }

        public ValueTask<StartImpersonationResponse> StartImpersonationAsync(StartImpersonationRequest request)
        {

            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == request.UniqueName &&
                    x.Organization == request.Organization);

            var response = new StartImpersonationResponse
            {
                Bearer = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{request.Organization}:{request.UniqueName}:{_appAuthentication.Bearer}"))
            };

            return ValueTask.FromResult(response);
        }

        public ValueTask<QueryAuthorizationsResponse> QueryAuthorizationsAsync()
        {
            var authorizationValues = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(_appState.CurrentOrganization) &&
                !string.IsNullOrWhiteSpace(_appState.CurrentInspector))
            {
                var organization = _organizationManager.GetQueryable()
                    .Single(x => x.UniqueName == _appState.CurrentOrganization);

                authorizationValues.Add("Inspector");

                if (organization.ChiefInspector == _appState.CurrentInspector)
                {
                    authorizationValues.Add("ChiefInspector");
                }

                if (_appSettings.Maintainer == _appState.CurrentInspector &&
                    _appSettings.MaintainerOrganization == _appState.CurrentOrganization)
                {
                    authorizationValues.Add("Maintainer");
                }

                if (!string.IsNullOrWhiteSpace(_appAuthentication.Impersonator))
                {
                    authorizationValues.Add("Impersonator");
                }
            }

            return ValueTask.FromResult(new QueryAuthorizationsResponse
            {
                Values = authorizationValues
            });
        }

        public ValueTask RegisterInspectorAsync(RegisterInspectorRequest request)
        {
            var organization = _organizationManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == request.Organization &&
                    x.Activated);

            return _inspectorManager.InsertAsync(new Inspector
            {
                MailAddress = request.MailAddress,
                Organization = organization.UniqueName,
                OrganizationActivated = organization.Activated,
                OrganizationDisplayName = organization.DisplayName,
                Secret = request.Secret,
                UniqueName = request.UniqueName,
                Activated = false
            });
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            var activateOrganisation = _organizationManager.GetQueryable().FirstOrDefault() == null;

            await _inspectorManager.InsertAsync(new Inspector
            {
                MailAddress = request.ChiefInspectorMail,
                Organization = request.UniqueName,
                OrganizationActivated = activateOrganisation,
                OrganizationDisplayName = request.DisplayName,
                Secret = request.ChiefInspectorSecret,
                UniqueName = request.ChiefInspectorName,
                Activated = true
            });
                        
            await _organizationManager.InsertAsync(new Organization
            {
                ChiefInspector = request.ChiefInspectorName,
                UniqueName = request.UniqueName,
                DisplayName = request.DisplayName,
                Activated = activateOrganisation,
            });
        }

        public async ValueTask<SignInInspectorResponse> SignInInspectorAsync(SignInInspectorRequest request)
        {

            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == request.UniqueName &&
                    x.Secret == request.Secret &&
                    x.Organization == request.Organization);

            inspector.Proof = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));

            await _inspectorManager.UpdateAsync(inspector);

            return new SignInInspectorResponse
            {
                Bearer = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{inspector.Organization}:{inspector.UniqueName}:{inspector.Proof}"))
            };
        }

        public async ValueTask SignOutInspectorAsync()
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == _appState.CurrentInspector &&
                    x.Organization == _appState.CurrentOrganization);

            inspector.Proof = $"{Guid.NewGuid()}";

            await _inspectorManager.UpdateAsync(inspector);
        }

        public ValueTask StopImpersonationAsync()
            => throw new NotSupportedException("As the server does not remember impersonation, an impersonation can not be stopped.");

        public async ValueTask RepairChiefInspectorAsync(RepairChiefInspectorRequest request)
        {
            var organization = await _organizationManager.GetAsync(request.Organization);

            var inspector = _inspectorManager.GetQueryable()
                .SingleOrDefault(x =>
                    x.UniqueName == organization.ChiefInspector);
            
            if (inspector != null)
            {
                inspector.Activated = true;
                await _inspectorManager.UpdateAsync(inspector);
            }

            inspector ??= new Inspector
            {
                Activated = true,
                Organization = request.Organization,
                Secret = "default",
                UniqueName = organization.ChiefInspector,
            };

            await _inspectorManager.InsertAsync(inspector);
        }

        public async ValueTask<AssessChiefInspectorDefectivenessResponse> AssessChiefInspectorDefectivenessAsync(AssessChiefInspectorDefectivenessRequest request)
        {
            var organization = await _organizationManager.GetAsync(request.Organization);
            
            var inspector = _inspectorManager.GetQueryable()
                .SingleOrDefault(x => 
                    x.UniqueName == organization.ChiefInspector &&
                    x.Activated);

            return new AssessChiefInspectorDefectivenessResponse
            {
                Defective = inspector == null,
            };
        }
    }
}