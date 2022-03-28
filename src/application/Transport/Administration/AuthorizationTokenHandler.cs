using Super.Paula.Authorization;
using Super.Paula.Environment;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Administration
{
    public class AuthorizationTokenHandler : IAuthorizationTokenHandler
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly AppSettings _appSettings;

        public AuthorizationTokenHandler(
            IOrganizationManager organizationManager,
            AppSettings appSettings)
        {
            _organizationManager = organizationManager;
            _appSettings = appSettings;
        }

        public void RewriteAuthorizations(Token token)
        {
            var authorizations = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(token.Organization))
            {
                var organization = _organizationManager.GetQueryable()
                    .Single(x => x.UniqueName == token.Organization);

                if (_appSettings.DemoIdentity == token.Identity)
                {
                    authorizations.Add("Observer");
                }
                else if (!string.IsNullOrWhiteSpace(token.Inspector))
                {
                    authorizations.Add("Inspector");
                }

                if (organization.ChiefInspector == token.Inspector)
                {
                    authorizations.Add("Chief");
                }

                if (!string.IsNullOrWhiteSpace(token.ImpersonatorOrganization) &&
                    !string.IsNullOrWhiteSpace(token.ImpersonatorInspector))
                {
                    authorizations.Add("Impersonator");
                }
            }

            if (_appSettings.MaintainerIdentity == token.Identity &&
                !authorizations.Contains("Observer") &&
                !authorizations.Contains("Impersonator"))
            {
                authorizations.Add("Maintainer");
            }

            token.Authorizations = authorizations.ToArray();
        }
    }
}
