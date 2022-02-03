using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Authorization
{
    public class PaulaAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => Task.FromResult(_Policies.IsAuthorizedPolicy);

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => Task.FromResult<AuthorizationPolicy?>(_Policies.IsAnonymousePolicy);

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
            => Task.FromResult(
                policyName switch
                {
                    "RequiresInspectorViewability" => _Policies.RequiresInspectorOwnershipPolicy,
                    "RequiresMaintainability" => _Policies.RequiresMaintainabilityPolicy,
                    "RequiresManageability" => _Policies.RequiresManageabilityPolicy,
                    "RequiresManagementViewability" => _Policies.RequiresManagementViewabilityPolicy,
                    "RequiresAuditability" => _Policies.RequiresAuditabilityPolicy,
                    "RequiresAuditingViewability" => _Policies.RequiresAuditingViewabilityPolicy,

                    "Impersonator" => _Policies.IsImpersonatorPolicy,
                    "Streamer" => _Policies.IsStreamerPolicy,
                    _ => null
                });
    }
}
