
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
                .AddRequirements(new PaulaAuthorizationRequirement())
                .Build();

        public static AuthorizationPolicy RequiresManageabilityPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Chief"))
                .Build();

        public static AuthorizationPolicy RequiresInspectabilityPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Inspector"))
                .Build();

        public static AuthorizationPolicy RequiresMaintainabilityPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Maintainer"))
                .Build();

        public static AuthorizationPolicy IsImpersonatorPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Impersonator"))
                .Build();

        public static AuthorizationPolicy IsStreamerPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Streamer"))
                .Build();

        public static AuthorizationPolicy RequiresWeekManageabilityPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Chief", "Observer"))
                .Build();

        public static AuthorizationPolicy RequiresWeekInspectabilityPolicy =>
            new AuthorizationPolicyBuilder()
                .AddRequirements(new PaulaAuthorizationRequirement("Inspector", "Observer"))
                .Build();

    }
}