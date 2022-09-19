using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace Super.Paula.Shared.Security;

public static class BadgeAuthorizationPolicies
{
    public static AuthorizationPolicy AnonymousPolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();

    public static AuthorizationPolicy AuthorizedPolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

    public static AuthorizationPolicy OnlyChiefPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Chief"))
            .Build();

    public static AuthorizationPolicy OnlyInspectorPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Inspector"))
            .Build();

    public static AuthorizationPolicy OnlyMaintainerPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Maintainer"))
            .Build();

    public static AuthorizationPolicy OnlyImpersonatorPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Impersonator"))
            .Build();

    public static AuthorizationPolicy OnlyChiefOrObserverPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Chief", "Observer"))
            .Build();

    public static AuthorizationPolicy OnlyInspectorOrObserverPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new AnyAuthorizationClaimRequirement("Inspector", "Observer"))
            .Build();

    public static AuthorizationPolicy OnlyChiefOrObserverOrInspectorOwnerPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new InspectorClaimResourceRequirement("Chief", "Observer"))
            .Build();

    public static AuthorizationPolicy OnlyMaintainerOrIdentityOwnerPolicy =>
        new AuthorizationPolicyBuilder()
            .AddRequirements(new IdentityClaimResourceRequirement("Maintainer"))
            .Build();


}