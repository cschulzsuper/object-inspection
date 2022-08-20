
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Shared.Authorization;

[SuppressMessage("Style", "IDE1006")]
public static class _Policies
{
    public static AuthorizationPolicy AnonymousePolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();

    public static AuthorizationPolicy AuthorizedPolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

    public static AuthorizationPolicy ManagementFullPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Chief"))
            .Build();

    public static AuthorizationPolicy AuditingFullPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Inspector"))
            .Build();

    public static AuthorizationPolicy MaintenancePolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Maintainer"))
            .Build();

    public static AuthorizationPolicy ImpersonationPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Impersonator"))
            .Build();

    public static AuthorizationPolicy ManagementReadPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Chief", "Observer"))
            .Build();

    public static AuthorizationPolicy AuditingLimitedPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Inspector", "Observer"))
            .Build();

    public static AuthorizationPolicy InspectorReadPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new InspectorClaimResourceRequirement("Chief", "Observer"))
            .Build();

    public static AuthorizationPolicy IdentityReadPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new IdentityClaimResourceRequirement("Maintainer"))
            .Build();


}