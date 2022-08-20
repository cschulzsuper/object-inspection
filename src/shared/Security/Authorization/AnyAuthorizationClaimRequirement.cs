using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Shared.Authorization;

public class AnyAuthorizationClaimRequirement : IAuthorizationRequirement
{
    public AnyAuthorizationClaimRequirement(params string[] authorizations)
    {
        Authorizations = authorizations;
    }

    public string[] Authorizations { get; }
}