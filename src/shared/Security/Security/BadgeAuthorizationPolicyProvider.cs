using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ChristianSchulz.ObjectInspection.Shared.Security;

public class BadgeAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => Task.FromResult(BadgeAuthorizationPolicies.AuthorizedPolicy);

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => Task.FromResult<AuthorizationPolicy?>(BadgeAuthorizationPolicies.AnonymousPolicy);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        => Task.FromResult(
            policyName switch
            {
                "OnlyChief" => BadgeAuthorizationPolicies.OnlyChiefPolicy,
                "OnlyChiefOrObserver" => BadgeAuthorizationPolicies.OnlyChiefOrObserverPolicy,
                "OnlyChiefOrObserverOrInspectorOwner" => BadgeAuthorizationPolicies.OnlyChiefOrObserverOrInspectorOwnerPolicy,
                "OnlyImpersonator" => BadgeAuthorizationPolicies.OnlyImpersonatorPolicy,
                "OnlyInspector" => BadgeAuthorizationPolicies.OnlyInspectorPolicy,
                "OnlyInspectorOrObserver" => BadgeAuthorizationPolicies.OnlyInspectorOrObserverPolicy,
                "OnlyMaintainer" => BadgeAuthorizationPolicies.OnlyMaintainerPolicy,
                "OnlyMaintainerOrIdentityOwner" => BadgeAuthorizationPolicies.OnlyMaintainerOrIdentityOwnerPolicy,

                _ => null
            });
}