using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Runtime;
using Super.Paula.Environment;

namespace Super.Paula.Application.Administration
{
    internal class AccountHandler : IAccountHandler
    {
        private readonly IInspectorManager _inspectorManager;
        private readonly IIdentityManager _identityManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly IConnectionManager _connectionManager;
        private readonly AppSettings _appSettings;
        private readonly AppAuthentication _appAuthentication;

        public AccountHandler(
            IInspectorManager inspectorManager,
            IIdentityManager identityManager,
            IOrganizationManager organizationManager,
            IConnectionManager connectionManager,
            AppSettings appSettings,
            AppAuthentication appAuthentication)
        {
            _inspectorManager = inspectorManager;
            _identityManager = identityManager;
            _organizationManager = organizationManager;
            _connectionManager = connectionManager;
            _appSettings = appSettings;
            _appAuthentication = appAuthentication;
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == _appAuthentication.Inspector &&
                    x.Organization == _appAuthentication.Organization);

            var identity = _identityManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == inspector.Identity &&
                    x.Secret == request.OldSecret);

            identity.Secret = request.NewSecret;

            await _identityManager.UpdateAsync(identity);
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

            if (!string.IsNullOrWhiteSpace(_appAuthentication.Organization) &&
                !string.IsNullOrWhiteSpace(_appAuthentication.Inspector))
            {
                var organization = _organizationManager.GetQueryable()
                    .Single(x => x.UniqueName == _appAuthentication.Organization);

                authorizationValues.Add("Inspector");

                if (organization.ChiefInspector == _appAuthentication.Inspector)
                {
                    authorizationValues.Add("ChiefInspector");
                }

                if (_appSettings.Maintainer == _appAuthentication.Inspector &&
                    _appSettings.MaintainerOrganization == _appAuthentication.Organization)
                {
                    authorizationValues.Add("Maintainer");
                }

                if (!string.IsNullOrWhiteSpace(_appAuthentication.ImpersonatorBearer))
                {
                    authorizationValues.Add("Impersonator");
                }
            }

            return ValueTask.FromResult(new QueryAuthorizationsResponse
            {
                Values = authorizationValues
            });
        }

        public async ValueTask RegisterInspectorAsync(RegisterInspectorRequest request)
        {
            var organization = _organizationManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == request.Organization &&
                    x.Activated);

            await _inspectorManager.InsertAsync(new Inspector
            {
                Identity = request.UniqueName,
                Organization = organization.UniqueName,
                OrganizationActivated = organization.Activated,
                OrganizationDisplayName = organization.DisplayName,
                UniqueName = request.UniqueName,
                Activated = false
            });

            await _identityManager.InsertAsync(new Identity
            {
                MailAddress = request.MailAddress,
                Secret = request.Secret,
                UniqueName = request.UniqueName
            });
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            var activateOrganization = _organizationManager.GetQueryable().FirstOrDefault() == null;

            await _organizationManager.InsertAsync(new Organization
            {
                ChiefInspector = request.ChiefInspector,
                UniqueName = request.UniqueName,
                DisplayName = request.DisplayName,
                Activated = activateOrganization,
            });

            await _inspectorManager.InsertAsync(new Inspector
            {
                Identity = request.ChiefInspector,
                Organization = request.UniqueName,
                OrganizationActivated = activateOrganization,
                OrganizationDisplayName = request.DisplayName,
                UniqueName = request.ChiefInspector,
                Activated = true
            });

            await _identityManager.InsertAsync(new Identity
            {
                MailAddress = request.ChiefInspectorMail,
                Secret = request.ChiefInspectorSecret,
                UniqueName = request.UniqueName
            });
        }

        public ValueTask<SignInInspectorResponse> SignInInspectorAsync(SignInInspectorRequest request)
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == request.UniqueName &&
                    x.Organization == request.Organization);

            _ = _identityManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == inspector.Identity &&
                    x.Secret == request.Secret);

            var connectionProof = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));

            _connectionManager.Trace(
                request.Organization,
                request.UniqueName,
                connectionProof);

            var response = new SignInInspectorResponse
            {
                Bearer = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{inspector.Organization}:{inspector.UniqueName}:{connectionProof}"))
            };

            return ValueTask.FromResult(response);
        }

        public async ValueTask SignOutInspectorAsync()
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == _appAuthentication.Inspector &&
                    x.Organization == _appAuthentication.Organization);

            _connectionManager.Forget(
                inspector.Organization,
                inspector.UniqueName);

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
            else
            {
                inspector = new Inspector
                {
                    Activated = true,
                    Organization = request.Organization,
                    UniqueName = organization.ChiefInspector,
                    Identity = organization.ChiefInspector
                };

                await _inspectorManager.InsertAsync(inspector);
            }

            var identity = _identityManager.GetQueryable()
                .SingleOrDefault(x =>
                    x.UniqueName == organization.ChiefInspector);

            if (identity != null)
            {
                identity.Secret = "default";
                await _identityManager.UpdateAsync(identity);
            }
            else
            {
                identity = new Identity
                {
                    Secret = "default",
                    MailAddress = string.Empty,
                    UniqueName = inspector.Identity
                };

                await _identityManager.InsertAsync(identity);
            }
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

        public ValueTask VerifyAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}