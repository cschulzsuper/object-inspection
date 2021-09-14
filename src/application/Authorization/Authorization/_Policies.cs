
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Authorization
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Policies
    {
        public static AuthorizationPolicy IsAnonymousePolicy =>
            new AuthorizationPolicyBuilder()
                .RequireAssertion(_ => true)
                .Build();

        public static AuthorizationPolicy IsAuthorizedPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement(string.Empty))
                .Build();

        public static AuthorizationPolicy IsChiefInspectorPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements( new PaulaAuthorizationRequirement("ChiefInspector"))
                .Build();

        public static AuthorizationPolicy IsInspectorPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Inspector"))
                .Build();

        public static AuthorizationPolicy IsMaintainerPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Maintainer"))
                .Build();

        public static AuthorizationPolicy IsImpersonatorPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Impersonator"))
                .Build();

    }
}