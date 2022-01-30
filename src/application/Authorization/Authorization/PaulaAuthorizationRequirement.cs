using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Authorization
{
    public class PaulaAuthorizationRequirement : IAuthorizationRequirement
    {
        public PaulaAuthorizationRequirement(params string[] authorizations)
        {
            Authorizations = authorizations;
        }

        public string[] Authorizations { get; }
    }
}