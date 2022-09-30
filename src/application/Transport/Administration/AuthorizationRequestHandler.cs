using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.BadgeSecurity;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class AuthorizationRequestHandler : IAuthorizationRequestHandler
{
    private readonly IInspectorManager _inspectorManager;
    private readonly IIdentityInspectorManager _identityInspectorManager;
    private readonly IBadgeHandler _badgeHandler;
    private readonly ClaimsPrincipal _user;

    public AuthorizationRequestHandler(
        IInspectorManager inspectorManager,
        IIdentityInspectorManager identityInspectorManager,
        IBadgeHandler badgeHandler,
        ClaimsPrincipal user)
    {
        _inspectorManager = inspectorManager;
        _identityInspectorManager = identityInspectorManager;
        _badgeHandler = badgeHandler;
        _user = user;
    }

    public ValueTask<string> AuthorizeAsync(string organization, string inspector)
    {
        var entity = _identityInspectorManager
            .GetIdentityBasedQueryable(_user.Claims.GetIdentity())
            .Single(x =>
                x.Activated &&
                x.Inspector == inspector &&
                x.Organization == organization);

        var badge = _badgeHandler.Authorize(_user, "inspector", entity);
        
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


        var badge = _badgeHandler.Authorize(_user, "impersonator", entity);

        return ValueTask.FromResult(badge);
    }

    public ValueTask<string> StopImpersonationAsync()
    {
        var entity = _identityInspectorManager.GetQueryable()
           .Single(x =>
               x.Activated &&
               x.Inspector == _user.Claims.GetImpersonatorInspector() &&
               x.Organization == _user.Claims.GetImpersonatorOrganization());

        var badge = _badgeHandler.Authorize(_user, "inspector", entity);

        return ValueTask.FromResult(badge);
    }
}