using Super.Paula.Application.Operation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Shared.Security;

namespace Super.Paula.Application.Administration;

public class AuthorizationRequestHandler : IAuthorizationRequestHandler
{
    private readonly IInspectorManager _inspectorManager;
    private readonly IIdentityInspectorManager _identityInspectorManager;
    private readonly IAuthorizationTokenHandler _authorizationTokenHandler;
    private readonly IConnectionManager _connectionManager;
    private readonly ClaimsPrincipal _user;

    public AuthorizationRequestHandler(
        IInspectorManager inspectorManager,
        IIdentityInspectorManager identityInspectorManager,
        IAuthorizationTokenHandler authorizationTokenHandler,
        IConnectionManager connectionManager,
        ClaimsPrincipal user)
    {
        _inspectorManager = inspectorManager;
        _identityInspectorManager = identityInspectorManager;
        _authorizationTokenHandler = authorizationTokenHandler;
        _connectionManager = connectionManager;
        _user = user;
    }

    public ValueTask<string> AuthorizeAsync(string organization, string inspector)
    {
        var identityInspector = _identityInspectorManager
            .GetIdentityBasedQueryable(_user.GetIdentity())
            .Single(x =>
                x.Activated &&
                x.Inspector == inspector &&
                x.Organization == organization);

        var token = _user.ToToken();

        token.Inspector = identityInspector.Inspector;
        token.Organization = identityInspector.Organization;

        _connectionManager.Trace(
            $"{token.Organization}:{token.Inspector}",
            token.Proof!);

        _authorizationTokenHandler.RewriteAuthorizations(token);

        return ValueTask.FromResult(token.ToBase64String());
    }

    public ValueTask<string> StartImpersonationAsync(string organization, string inspector)
    {
        var entity = _inspectorManager.GetQueryable()
            .Single(x =>
                x.Activated &&
                x.OrganizationActivated &&
                x.UniqueName == inspector &&
                x.Organization == organization);

        var token = _user.ToToken();

        token.Proof = _user.GetProof();
        token.Inspector = entity.UniqueName;
        token.Organization = entity.Organization;
        token.ImpersonatorInspector = _user.GetInspector();
        token.ImpersonatorOrganization = _user.GetOrganization();

        _authorizationTokenHandler.RewriteAuthorizations(token);

        return ValueTask.FromResult(token.ToBase64String());
    }

    public ValueTask<string> StopImpersonationAsync()
    {
        var inspector = _identityInspectorManager.GetQueryable()
           .Single(x =>
               x.Activated &&
               x.UniqueName == _user.GetImpersonatorInspector() &&
               x.Organization == _user.GetImpersonatorOrganization());

        var token = _user.ToToken();

        token.Inspector = inspector.UniqueName;
        token.Organization = inspector.Organization;
        token.ImpersonatorInspector = null;
        token.ImpersonatorOrganization = null;

        _authorizationTokenHandler.RewriteAuthorizations(token);

        return ValueTask.FromResult(token.ToBase64String());
    }
}