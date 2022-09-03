using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Authentication;
using Super.Paula.Application.Operation;
using Super.Paula.Shared.Environment;
using Super.Paula.Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Super.Paula.Server.Security
{
    public class BadgeAuthenticationTracker : IBadgeAuthenticationTracker
    {
        private readonly AppSettings _appSettings;
        private readonly IConnectionManager _connectionManager;
        private readonly IOrganizationManager _organizationManager;

        public BadgeAuthenticationTracker(
            AppSettings appSettings,
            IConnectionManager connectionManager,
            IOrganizationManager organizationManager)
        {
            _connectionManager = connectionManager;
            _organizationManager = organizationManager;
            _appSettings = appSettings;
        }

        public ClaimsPrincipal? Verify(string? encodedBadge)
        {
            if (string.IsNullOrWhiteSpace(encodedBadge))
            {
                return null;
            }

            var decodedBadge = encodedBadge.ToBadge();
            if (decodedBadge == null)
            {
                return null;
            }

            if (decodedBadge.Identity == null || decodedBadge.Proof == null)
            {
                return null;
            }

            var connectionAccount = decodedBadge.Identity;
            var connectionProof = decodedBadge.Proof;
            var connectionProofType = ConnectionProofTypes.Authentication;

            var validIdentity = _connectionManager.Verify(connectionAccount, connectionProof, connectionProofType);

            if (!validIdentity)
            {
                if (decodedBadge.Organization != null &&
                    decodedBadge.Inspector != null)
                {

                    connectionAccount = $"{decodedBadge.Organization}:{decodedBadge.Inspector}";
                    connectionProof = decodedBadge.Proof;
                    connectionProofType = ConnectionProofTypes.Authorization;

                    var validInspector = _connectionManager.Verify(connectionAccount, connectionProof, connectionProofType);

                    if (!validInspector &&
                        _appSettings.MaintainerIdentity == decodedBadge.Identity)
                    {
                        connectionAccount = $"{decodedBadge.ImpersonatorOrganization}:{decodedBadge.ImpersonatorInspector}";

                        validInspector = _connectionManager.Verify(connectionAccount, connectionProof, connectionProofType);
                    }

                    if (!validInspector)
                    {
                        return null;
                    }
                }
            }

            RewriteAuthorizations(decodedBadge);

            var claims = decodedBadge.ToClaims();
            var claimsIdentity = new ClaimsIdentity(claims, "badge");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public string Trace(ClaimsPrincipal user, string badgeType, object badgeResource)
            => badgeType switch
            {
                "identity" 
                    => SwitchToIdentity(user, badgeResource),

                "inspector" 
                    => SwitchToInspector(user, badgeResource),

                "impersonation" 
                    => SwitchToImpersonator(user, badgeResource),

                _ => string.Empty
            };

        public void Forget(ClaimsPrincipal user)
        {
            var connectionAccount = user.GetIdentity();
            _connectionManager.Forget(connectionAccount);

            connectionAccount = $"{user.GetOrganization()}:{user.GetInspector()}";
            _connectionManager.Forget(connectionAccount);
        }

        private string SwitchToIdentity(ClaimsPrincipal _, object badgeResource)
        {
            var identity = (Identity)badgeResource;
            
            var connectionAccount = identity.UniqueName;
            var connectionProof = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));
            var connectionProofType = ConnectionProofTypes.Authentication;

            _connectionManager.Trace(connectionAccount, connectionProof, connectionProofType);

            var badge = new Badge
            {
                Identity = identity.UniqueName,
                Proof = connectionProof
            };

            return badge.ToBase64String();
        }

        public string SwitchToInspector(ClaimsPrincipal user, object badgeResource)
        {
            var badge = user.ToBadge();

            var identityInspector = (IdentityInspector)badgeResource;

            var connectionAccount = $"{identityInspector.Organization}:{identityInspector.Inspector}";
            var connectionProof = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));
            var connectionProofType = ConnectionProofTypes.Authorization;

            _connectionManager.Trace(connectionAccount, connectionProof, connectionProofType);

            badge.Proof = connectionProof;
            badge.Identity = identityInspector.UniqueName;
            badge.Inspector = identityInspector.Inspector;
            badge.Organization = identityInspector.Organization;

            RewriteAuthorizations(badge);

            return badge.ToBase64String();
        }

        private string SwitchToImpersonator(ClaimsPrincipal user, object badgeResource)
        {
            var badge = user.ToBadge();

            var inspector = (Inspector)badgeResource;

            var connectionAccount = $"{badge.Organization}:{badge.Inspector}";
            var connectionProof = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));
            var connectionProofType = ConnectionProofTypes.Authorization;

            _connectionManager.Trace(connectionAccount, connectionProof, connectionProofType);

            badge.Proof = connectionProof;
            badge.ImpersonatorInspector = badge.Inspector;
            badge.ImpersonatorOrganization = badge.Organization;
            badge.Inspector = inspector.UniqueName;
            badge.Organization = inspector.Organization;

            RewriteAuthorizations(badge);

            return badge.ToBase64String();
        }

        private void RewriteAuthorizations(Badge badge)
        {
            var authorizations = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(badge.Organization))
            {
                var organization = _organizationManager.Get(badge.Organization);

                if (_appSettings.DemoIdentity == badge.Identity)
                {
                    authorizations.Add("Observer");
                }
                else if (!string.IsNullOrWhiteSpace(badge.Inspector))
                {
                    authorizations.Add("Inspector");
                }

                if (organization.ChiefInspector == badge.Inspector)
                {
                    authorizations.Add("Chief");
                }

                if (!string.IsNullOrWhiteSpace(badge.ImpersonatorOrganization) &&
                    !string.IsNullOrWhiteSpace(badge.ImpersonatorInspector))
                {
                    authorizations.Add("Impersonator");
                }
            }

            if (_appSettings.MaintainerIdentity == badge.Identity &&
                !authorizations.Contains("Observer") &&
                !authorizations.Contains("Impersonator"))
            {
                authorizations.Add("Maintainer");
            }

            badge.Authorizations = authorizations.ToArray();
        }
    }
}
