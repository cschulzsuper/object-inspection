using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Shared.Security;

namespace Super.Paula.Application.Administration;

public class AuthorizationRequestHandler : IAuthorizationRequestHandler
{
    private readonly IInspectorManager _inspectorManager;
    private readonly IIdentityInspectorManager _identityInspectorManager;
    private readonly IBadgeAuthenticationTracker _badgeAuthenticationTracker;
    private readonly ClaimsPrincipal _user;

    public AuthorizationRequestHandler(
        IInspectorManager inspectorManager,
        IIdentityInspectorManager identityInspectorManager,
        IBadgeAuthenticationTracker badgeAuthenticationTracker,
        ClaimsPrincipal user)
    {
        _inspectorManager = inspectorManager;
        _identityInspectorManager = identityInspectorManager;
        _badgeAuthenticationTracker = badgeAuthenticationTracker;
        _user = user;
    }

    public ValueTask<string> AuthorizeAsync(string organization, string inspector)
    {
        var entity = _identityInspectorManager
            .GetIdentityBasedQueryable(_user.GetIdentity())
            .Single(x =>
                x.Activated &&
                x.Inspector == inspector &&
                x.Organization == organization);

        var badge = _badgeAuthenticationTracker.Trace(_user, "inspector", entity);
        
        return ValueTask.FromResult(badge);
    }

    public ValueTask<string> StartImpersonationAsync(string organization, string inspector)
    {
        var entity = _inspectorManager.GetQueryable()
            .Single(x =>
                x.Activated &&
                x.OrganizationActivated &&
                x.UniqueName == inspector &&
                x.Organization == organization);


        var badge = _badgeAuthenticationTracker.Trace(_user, "impersonator", entity);

        return ValueTask.FromResult(badge);
    }

    public ValueTask<string> StopImpersonationAsync()
    {
        var entity = _identityInspectorManager.GetQueryable()
           .Single(x =>
               x.Activated &&
               x.Inspector == _user.GetImpersonatorInspector() &&
               x.Organization == _user.GetImpersonatorOrganization());

        var badge = _badgeAuthenticationTracker.Trace(_user, "inspector", entity);

        return ValueTask.FromResult(badge);
    }
}