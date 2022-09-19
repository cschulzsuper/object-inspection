using Super.Paula.Application.Administration;
using Super.Paula.Client.Storage;
using System.Threading.Tasks;
using Super.Paula.BadgeUsage;
using Super.Paula.Client.Security;
using Super.Paula.Shared.Security;

namespace Super.Paula.Client.Administration;

public class ExtendedAuthorizationRequestHandler : IAuthorizationRequestHandler
{
    private readonly IAuthorizationRequestHandler _authorizationHandler;
    private readonly BadgeStorage _badgeStorage;

    public ExtendedAuthorizationRequestHandler(
        IAuthorizationRequestHandler authorizationHandler,
        BadgeStorage badgeStorage)
    {
        _authorizationHandler = authorizationHandler;
        _badgeStorage = badgeStorage;
    }

    public async ValueTask<string> AuthorizeAsync(string organization, string inspector)
    {
        var response = await _authorizationHandler.AuthorizeAsync(organization, inspector);

        await _badgeStorage.SetAsync(response);

        return response;
    }

    public async ValueTask<string> StartImpersonationAsync(string organization, string inspector)
    {
        var response = await _authorizationHandler.StartImpersonationAsync(organization, inspector);

        await _badgeStorage.SetAsync(response);

        return response;
    }

    public async ValueTask<string> StopImpersonationAsync()
    {
        var response = await _authorizationHandler.StopImpersonationAsync();

        await _badgeStorage.SetAsync(response);

        return response;
    }
}