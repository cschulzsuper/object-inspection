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
                    "RequiresMaintainability" => _Policies.RequiresMaintainabilityPolicy,
                    "RequiresManageability" => _Policies.RequiresManageabilityPolicy,
                    "RequiresWeekManageability" => _Policies.RequiresWeekManageabilityPolicy,
                    "RequiresInspectability" => _Policies.RequiresInspectabilityPolicy,
                    "RequiresWeekInspectability" => _Policies.RequiresWeekInspectabilityPolicy,

                    "Impersonator" => _Policies.IsImpersonatorPolicy,
                    "Streamer" => _Policies.IsStreamerPolicy,
                    _ => null
                });
    }
}
