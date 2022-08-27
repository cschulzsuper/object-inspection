using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Authorization;

public class PaulaAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => Task.FromResult(_Policies.AuthorizedPolicy);

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => Task.FromResult<AuthorizationPolicy?>(_Policies.AnonymousePolicy);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        => Task.FromResult(
            policyName switch
            {
                "OnlyChief" => _Policies.OnlyChiefPolicy,
                "OnlyChiefOrObserver" => _Policies.OnlyChiefOrObserverPolicy,
                "OnlyChiefOrObserverOrInspectorOwner" => _Policies.OnlyChiefOrObserverOrInspectorOwnerPolicy,
                "OnlyImpersonator" => _Policies.OnlyImpersonatorPolicy,
                "OnlyInspector" => _Policies.OnlyInspectorPolicy,
                "OnlyInspectorOrObserver" => _Policies.OnlyInspectorOrObserverPolicy,
                "OnlyMaintainer" => _Policies.OnlyMaintainerPolicy,
                "OnlyMaintainerOrIdentityOwner" => _Policies.OnlyMaintainerOrIdentityOwnerPolicy,

                _ => null
            });
}