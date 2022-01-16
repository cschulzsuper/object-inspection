using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Runtime;
using Super.Paula.Environment;

namespace Super.Paula.Application.Administration
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IInspectorManager _inspectorManager;
        private readonly IIdentityManager _identityManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly IConnectionManager _connectionManager;
        private readonly IPasswordHasher<Identity> _passwordHasher;
        private readonly AppSettings _appSettings;
        private readonly AppAuthentication _appAuthentication;
        private readonly ITokenAuthorizationFilter _tokenAuthorizatioFilter;

        public AccountHandler(
            IInspectorManager inspectorManager,
            IIdentityManager identityManager,
            IOrganizationManager organizationManager,
            IConnectionManager connectionManager,
            IPasswordHasher<Identity> passwordHasher,
            AppSettings appSettings,
            AppAuthentication appAuthentication,
            ITokenAuthorizationFilter tokenAuthorizatioFilter)
        {
            _inspectorManager = inspectorManager;
            _identityManager = identityManager;
            _organizationManager = organizationManager;
            _connectionManager = connectionManager;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings;
            _appAuthentication = appAuthentication;
            _tokenAuthorizatioFilter = tokenAuthorizatioFilter;
        }

        public async ValueTask ChangeSecretAsync(ChangeSecretRequest request)
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.UniqueName == _appAuthentication.Inspector &&
                    x.Organization == _appAuthentication.Organization);

            var identity = await _identityManager.GetAsync(inspector.Identity);

            var oldSecretVerification = _passwordHasher.VerifyHashedPassword(identity, identity.Secret, request.OldSecret);
            if(oldSecretVerification == PasswordVerificationResult.Failed)
            {
                throw new TransportException($"The old secret does not match");
            }

            identity.Secret = _passwordHasher.HashPassword(identity, request.NewSecret);

            await _identityManager.UpdateAsync(identity);
        }

        public ValueTask<string> StartImpersonationAsync(StartImpersonationRequest request)
        {

            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == request.UniqueName &&
                    x.Organization == request.Organization);

            var token = new Token
            {
                Inspector = request.UniqueName,
                Organization = request.Organization,
                Proof = _appAuthentication.Proof,
                ImpersonatorInspector = _appAuthentication.Inspector,
                ImpersonatorOrganization = _appAuthentication.Organization
            };

            _tokenAuthorizatioFilter.Apply(token);

            return ValueTask.FromResult(token.ToBase64String());
        }

        public async IAsyncEnumerable<AccountAuthorizationResponse> GetAuthorizations()
        {
            await Task.CompletedTask;

            var token = new Token
            {
                Inspector = _appAuthentication.Inspector,
                Organization = _appAuthentication.Organization,
                Proof = _appAuthentication.Proof,
                ImpersonatorInspector = _appAuthentication.ImpersonatorInspector,
                ImpersonatorOrganization = _appAuthentication.ImpersonatorOrganization
            };

            _tokenAuthorizatioFilter.Apply(token);

            foreach (var authorization in token.Authorizations)
            {
                yield return new AccountAuthorizationResponse
                {
                    Value = authorization
                };
            }
        }

        public async ValueTask RegisterIdentityAsync(RegisterIdentityRequest request)
        {
            var identity = new Identity
            {
                MailAddress = request.MailAddress,
                UniqueName = request.UniqueName
            };

            identity.Secret = _passwordHasher.HashPassword(identity, request.Secret);

            await _identityManager.InsertAsync(identity);
        }

        public async ValueTask RegisterOrganizationAsync(RegisterOrganizationRequest request)
        {
            if (_appSettings.Maintainer != request.ChiefInspector)
            {
                throw new TransportException($"Only the maintainer can register with an organization");
            }

            if (_appSettings.MaintainerOrganization != request.UniqueName)
            {
                throw new TransportException($"Only the maintainer organization can be registered");
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
                Identity = request.ChiefInspector,
                Organization = request.UniqueName,
                OrganizationActivated = true,
                OrganizationDisplayName = request.DisplayName,
                UniqueName = request.ChiefInspector,
                Activated = true
            });

            var identity = new Identity
            {
                MailAddress = request.ChiefInspectorMail,
                UniqueName = request.UniqueName
            };

            identity.Secret = _passwordHasher.HashPassword(identity, request.ChiefInspectorSecret);

            await _identityManager.InsertAsync(identity);
        }

        public async ValueTask<string> SignInInspectorAsync(SignInInspectorRequest request)
        {
            var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == request.UniqueName &&
                    x.Organization == request.Organization);

            var identity = await _identityManager.GetAsync(inspector.Identity);

            var secretVerification = _passwordHasher.VerifyHashedPassword(identity, identity.Secret, request.Secret);
            switch (secretVerification)
            {
                case PasswordVerificationResult.Success:
                    break;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    identity.Secret = _passwordHasher.HashPassword(identity, request.Secret);
                    await _identityManager.UpdateAsync(identity);
                    break;

                case PasswordVerificationResult.Failed:
                    throw new TransportException($"The secret does not match");
            }

            var connectionProof = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));

            _connectionManager.Trace(
                request.Organization,
                request.UniqueName,
                connectionProof);

            var token = new Token
            {
                Inspector = inspector.UniqueName,
                Organization = inspector.Organization,
                Proof = connectionProof
            };

            _tokenAuthorizatioFilter.Apply(token);

            return token.ToBase64String();
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

        public ValueTask<string> StopImpersonationAsync()
        {
             var inspector = _inspectorManager.GetQueryable()
                .Single(x =>
                    x.Activated &&
                    x.OrganizationActivated &&
                    x.UniqueName == _appAuthentication.ImpersonatorInspector &&
                    x.Organization == _appAuthentication.ImpersonatorOrganization);

            var token = new Token
            {
                Inspector = inspector.UniqueName,
                Organization = inspector.Organization,
                Proof = _appAuthentication.Proof
            };

            _tokenAuthorizatioFilter.Apply(token);

            return ValueTask.FromResult(token.ToBase64String());
        }

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
                    OrganizationDisplayName = organization.DisplayName,
                    UniqueName = organization.ChiefInspector,
                    Identity = string.Empty
                };

                await _inspectorManager.InsertAsync(inspector);
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
                Defective = inspector == null
            };
        }

        public ValueTask VerifyAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}