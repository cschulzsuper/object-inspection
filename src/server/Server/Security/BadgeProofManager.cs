using System;
using System.Security.Claims;
using System.Text;
using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Authentication;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.BadgeSecurity;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Server.Security;

public class BadgeProofManager : IBadgeProofManager
{
    private readonly IConnectionManager _connectionManager;
    private readonly AppSettings _appSettings;
    private readonly IOrganizationManager _organizationManager;

    public BadgeProofManager(
        IConnectionManager connectionManager, 
        AppSettings appSettings,
        IOrganizationManager organizationManager)
    {
        _connectionManager = connectionManager;
        _appSettings = appSettings;
        _organizationManager = organizationManager;
    }

    public string Create(BadgeProofAuthorizationContext context)
        => context.AuthorizationType switch
        {
            "identity" => CreateIdentity(context),
            "inspector" => CreateInspector(context),
            "impersonation" => CreateImpersonator(context),
            _ => string.Empty
        };

    private string CreateIdentity(BadgeProofAuthorizationContext context)
    {
        var identity = (Identity)context.AuthorizationResource;

        var connectionAccount = identity.UniqueName;
        var connectionProof = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));
        var connectionProofType = ConnectionProofTypes.Authentication;

        _connectionManager.Trace(connectionAccount, connectionProof, connectionProofType);

        return connectionProof;
    }

    public string CreateInspector(BadgeProofAuthorizationContext context)
    {
        var identityInspector = (IdentityInspector)context.AuthorizationResource;

        var connectionAccount = $"{identityInspector.Organization}:{identityInspector.Inspector}";
        var connectionProof = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));
        var connectionProofType = ConnectionProofTypes.Authorization;

        _connectionManager.Trace(connectionAccount, connectionProof, connectionProofType);

        return connectionProof;
    }

    private string CreateImpersonator(BadgeProofAuthorizationContext context)
    {
        var connectionAccount = $"{context.User.Claims.GetImpersonatorOrganization()}:{context.User.Claims.GetImpersonatorInspector()}";
        var connectionProof = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"));
        var connectionProofType = ConnectionProofTypes.Authorization;

        _connectionManager.Trace(connectionAccount, connectionProof, connectionProofType);

        return connectionProof;
    }

    public void Purge(ClaimsPrincipal user)
    {
        var connectionAccount = user.Claims.GetIdentity();
        _connectionManager.Forget(connectionAccount);

        connectionAccount = $"{user.Claims.GetOrganization()}:{user.Claims.GetInspector()}";
        _connectionManager.Forget(connectionAccount);
    }

    public bool Verify(BadgeProofAuthenticationContext context)
    {
        if (!context.Claims.HasIdentity() ||
            !context.Claims.HasProof())
        {
            return false;
        }

        var connectionAccount = context.Claims.GetIdentity();
        var connectionProof = context.Claims.GetProof();
        var connectionProofType = ConnectionProofTypes.Authentication;

        var validIdentity = _connectionManager.Verify(connectionAccount, connectionProof, connectionProofType);

        if (validIdentity)
        {
            return true;
        }

        if (context.Claims.HasOrganization() &&
            context.Claims.HasInspector())
        {
            var userOrganization = context.Claims.GetOrganization();
            var userInspector = context.Claims.GetInspector();

            connectionAccount = $"{userOrganization}:{userInspector}";
            connectionProof = context.Claims.GetProof();
            connectionProofType = ConnectionProofTypes.Authorization;

            var validInspector = _connectionManager.Verify(connectionAccount, connectionProof, connectionProofType);

            if (validInspector)
            {
                return IsOrganizationActivated(userOrganization);
            }

            if (_appSettings.MaintainerIdentity != context.Claims.GetIdentity())
            {
                return false;
            }

            userOrganization = context.Claims.GetImpersonatorOrganization();
            userInspector = context.Claims.GetImpersonatorInspector();

            connectionAccount = $"{userOrganization}:{userInspector}";

            validInspector = _connectionManager.Verify(connectionAccount, connectionProof, connectionProofType);

            if (validInspector)
            {
                return IsOrganizationActivated(userOrganization);
            }
        }

        return false;
    }

    private bool IsOrganizationActivated(string organization)
    {
        return _organizationManager
            .Get(organization).Activated;
    }
}