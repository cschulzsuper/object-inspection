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
                "InspectorRead" => _Policies.InspectorReadPolicy,
                "IdentityRead" => _Policies.IdentityReadPolicy,
                "Maintenance" => _Policies.MaintenancePolicy,
                "ManagementFull" => _Policies.ManagementFullPolicy,
                "ManagementRead" => _Policies.ManagementReadPolicy,
                "AuditingFull" => _Policies.AuditingFullPolicy,
                "AuditingLimited" => _Policies.AuditingLimitedPolicy,

                "Impersonation" => _Policies.ImpersonationPolicy,
                _ => null
            });
}