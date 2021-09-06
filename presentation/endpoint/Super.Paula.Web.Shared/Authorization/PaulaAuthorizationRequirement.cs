using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Web.Shared.Authorization
{
    public class PaulaAuthorizationRequirement : IAuthorizationRequirement
    {
        public PaulaAuthorizationRequirement(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}