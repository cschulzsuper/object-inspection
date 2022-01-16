using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Administration
{
    public class TokenAuthorizationFilter : ITokenAuthorizationFilter
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly AppSettings _appSettings;

        public TokenAuthorizationFilter(
            IOrganizationManager organizationManager, 
            AppSettings appSettings)
        {
            _organizationManager = organizationManager;
            _appSettings = appSettings;
        }

        public void Apply(Token token)
        {
            var authorizations = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(token.Organization))
            {
                var organization = _organizationManager.GetQueryable()
                    .Single(x => x.UniqueName == token.Organization);

                authorizations.Add("Inspector");

                if (organization.ChiefInspector == token.Inspector)
                {
                    authorizations.Add("ChiefInspector");
                }

                if (_appSettings.Maintainer == token.Inspector &&
                    _appSettings.MaintainerOrganization == token.Organization)
                {
                    authorizations.Add("Maintainer");
                }

                if (!string.IsNullOrWhiteSpace(token.ImpersonatorOrganization) &&
                    !string.IsNullOrWhiteSpace(token.ImpersonatorInspector))
                {
                    authorizations.Add("Impersonator");
                }
            }

            token.Authorizations = authorizations.ToArray();
        }
    }
}
