using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Shared.Security;

public class IdentityClaimResourceRequirement : IAuthorizationRequirement
{
    public IdentityClaimResourceRequirement(params string[] authorizations)
    {
        Authorizations = authorizations;
    }

    public string[] Authorizations { get; }
}