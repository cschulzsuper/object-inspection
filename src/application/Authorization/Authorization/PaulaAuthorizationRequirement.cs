using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Authorization
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