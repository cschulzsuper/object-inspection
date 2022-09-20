using ChristianSchulz.ObjectInspection.Application.Administration.Continuation;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class OrganizationContinuationService : IOrganizationContinuationService
{
    private readonly IContinuationStorage _continuationStorage;
    private readonly ClaimsPrincipal _user;

    public OrganizationContinuationService(
        IContinuationStorage continuationStorage,
        ClaimsPrincipal user)
    {
        _continuationStorage = continuationStorage;
        _user = user;
    }

    public async ValueTask AddCreateInspectorContinuationForChiefInspectorAsync(Organization entity)
    {
        var continuation = new CreateInspectorContinuation(
            entity.UniqueName,
            entity.ChiefInspector,
            _user.Claims.GetIdentity(),
            entity.Activated,
            entity.DisplayName,
            true);

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>
                {
                new Claim("Organization", entity.UniqueName)
                }));

        await _continuationStorage.AddAsync(continuation, user);
    }
}