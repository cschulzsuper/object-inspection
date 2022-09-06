using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Shared.Security;

public class InspectorClaimResourceRequirement : IAuthorizationRequirement
{
    public InspectorClaimResourceRequirement(params string[] authorizations)
    {
        Authorizations = authorizations;
    }

    public string[] Authorizations { get; }
}