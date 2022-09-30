using Microsoft.AspNetCore.Authorization;

namespace ChristianSchulz.ObjectInspection.Shared.Security;

public class AnyAuthorizationClaimRequirement : IAuthorizationRequirement
{
    public AnyAuthorizationClaimRequirement(params string[] authorizations)
    {
        Authorizations = authorizations;
    }

    public string[] Authorizations { get; }
}