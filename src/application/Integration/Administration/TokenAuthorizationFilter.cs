using Super.Paula.Authorization;
using Super.Paula.Environment;
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

                if (_appSettings.MaintainerIdentity == token.Identity)
                {
                    authorizations.Add("Maintainer");
                }

                if (!string.IsNullOrWhiteSpace(token.ImpersonatorOrganization) &&
                    !string.IsNullOrWhiteSpace(token.ImpersonatorInspector))
                {
                    authorizations.Add("Impersonator");
                }
            }

            if (!string.IsNullOrWhiteSpace(_appSettings.StreamerSecret) &&
                _appSettings.StreamerSecret == token.StreamerSecret)
            {
                authorizations.Add("Streamer");
            }

            token.Authorizations = authorizations.ToArray();
        }
    }
}
