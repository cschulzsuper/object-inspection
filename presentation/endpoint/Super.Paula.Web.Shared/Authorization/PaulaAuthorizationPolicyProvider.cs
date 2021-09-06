using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Super.Paula.Web.Shared.Authorization
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
                    "ChiefInspector" => _Policies.IsChiefInspectorPolicy,
                    "Inspector" => _Policies.IsInspectorPolicy,
                    "Maintainer" => _Policies.IsMaintainerPolicy,
                    "Impersonator" => _Policies.IsImpersonatorPolicy,
                    _ => null
                });
    }
}
