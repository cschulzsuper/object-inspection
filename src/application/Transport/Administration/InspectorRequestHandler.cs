using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Shared.Security;

namespace Super.Paula.Application.Administration;

public class InspectorRequestHandler : IInspectorRequestHandler
{
    private readonly IInspectorManager _inspectorManager;
    private readonly IOrganizationManager _organizationManager;
    private readonly IIdentityInspectorManager _identityInspectorManager;
    private readonly ClaimsPrincipal _user;
    private readonly IInspectorContinuationService _inspectorContinuationService;

    public InspectorRequestHandler(
        IInspectorManager inspectorManager,
        IOrganizationManager organizationManager,
        IIdentityInspectorManager identityInspectorManager,
        ClaimsPrincipal claimsPrincipal,
        IInspectorContinuationService inspectorContinuationService)
    {
        _inspectorManager = inspectorManager;
        _organizationManager = organizationManager;
        _identityInspectorManager = identityInspectorManager;
        _user = claimsPrincipal;
        _inspectorContinuationService = inspectorContinuationService;
    }

    public async ValueTask<InspectorResponse> CreateAsync(InspectorRequest request)
    {
        var organization = await _organizationManager.GetAsync(_user.Claims.GetOrganization());

        var entity = new Inspector
        {
            Identity = request.Identity,
            UniqueName = request.UniqueName,
            Activated = request.Activated,
            Organization = organization.UniqueName,
            OrganizationActivated = organization.Activated,
            OrganizationDisplayName = organization.DisplayName
        };

        await _inspectorManager.InsertAsync(entity);
        await _inspectorContinuationService.AddCreateIdentityInspectorContinuationAsync(entity);

        return new InspectorResponse
        {
            Identity = entity.Identity,
            UniqueName = entity.UniqueName,
            Activated = entity.Activated,
            BusinessObjects = entity.BusinessObjects.ToResponse(),
            ETag = entity.ETag
        };
    }

    public async ValueTask DeleteAsync(string inspector, string etag)
    {
        var entity = await _inspectorManager.GetAsync(inspector);

        entity.ETag = etag;

        await _inspectorManager.DeleteAsync(entity);
        await _inspectorContinuationService.AddDeleteIdentityInspectorContinuationAsync(entity);

        var identity = await _identityInspectorManager.GetAsync(
            entity.Identity,
            entity.Organization,
            entity.UniqueName);
        await _identityInspectorManager.DeleteAsync(identity);
    }

    public IAsyncEnumerable<InspectorResponse> GetAll()
        => _inspectorManager
            .GetAsyncEnumerable(query => query
            .Select(entity => new InspectorResponse
            {
                Identity = entity.Identity,
                UniqueName = entity.UniqueName,
                Activated = entity.Activated,
                BusinessObjects = entity.BusinessObjects.ToResponse(),
                ETag = entity.ETag
            }));

    public async ValueTask<InspectorResponse> GetAsync(string inspector)
    {
        var entity = await _inspectorManager.GetAsync(inspector);

        return new InspectorResponse
        {
            Identity = entity.Identity,
            UniqueName = entity.UniqueName,
            Activated = entity.Activated,
            BusinessObjects = entity.BusinessObjects.ToResponse(),
            ETag = entity.ETag
        };
    }

    public async ValueTask<InspectorResponse> GetCurrentAsync()
    {
        var entity = await _inspectorManager.GetAsync(_user.Claims.GetInspector());

        return new InspectorResponse
        {
            Identity = entity.Identity,
            UniqueName = entity.UniqueName,
            Activated = entity.Activated,
            BusinessObjects = entity.BusinessObjects.ToResponse(),
            ETag = entity.ETag
        };
    }

    public async ValueTask ReplaceAsync(string inspector, InspectorRequest request)
    {
        var entity = await _inspectorManager.GetAsync(inspector);

        var oldIdentity = entity.Identity;

        entity.Identity = request.Identity;
        entity.UniqueName = request.UniqueName;
        entity.Activated = request.Activated;
        entity.ETag = request.ETag;

        await _inspectorManager.UpdateAsync(entity);

        await _inspectorContinuationService.AddDeleteIdentityInspectorContinuationAsync(oldIdentity, entity.Organization, entity.UniqueName);
        await _inspectorContinuationService.AddCreateIdentityInspectorContinuationAsync(entity);
    }

    public async ValueTask<ActivateInspectorResponse> ActivateAsync(string inspector, string etag)
    {
        var entity = await _inspectorManager.GetAsync(inspector);

        entity.Activated = true;
        entity.ETag = etag;

        await _inspectorManager.UpdateAsync(entity);
        await _inspectorContinuationService.AddActivateIdentityInspectorContinuationAsync(entity);

        var identity = await _identityInspectorManager.GetAsync(
            entity.Identity,
            entity.Organization,
            entity.UniqueName);

        identity.Activated = entity.OrganizationActivated;

        await _identityInspectorManager.UpdateAsync(identity);

        return new ActivateInspectorResponse
        {
            ETag = entity.ETag
        };
    }

    public async ValueTask<DeactivateInspectorResponse> DeactivateAsync(string inspector, string etag)
    {
        var entity = await _inspectorManager.GetAsync(inspector);

        entity.Activated = false;
        entity.ETag = etag;

        await _inspectorManager.UpdateAsync(entity);
        await _inspectorContinuationService.AddDeactivateIdentityInspectorContinuationAsync(entity);

        var identity = await _identityInspectorManager.GetAsync(
            entity.Identity,
            entity.Organization,
            entity.UniqueName);

        identity.Activated = false;

        await _identityInspectorManager.UpdateAsync(identity);

        return new DeactivateInspectorResponse
        {
            ETag = entity.ETag
        };
    }

    public IAsyncEnumerable<InspectorResponse> GetAllForOrganization(string organization)
        => _inspectorManager
           .GetAsyncEnumerable(query => query
               .Where(x => x.Organization == organization)
               .Select(entity => new InspectorResponse
               {
                   Identity = entity.Identity,
                   UniqueName = entity.UniqueName,
                   Activated = entity.Activated,
                   BusinessObjects = entity.BusinessObjects.ToResponse(),
                   ETag = entity.ETag
               }));

    public IAsyncEnumerable<IdentityInspectorResponse> GetAllForIdentity(string identity)
        => _identityInspectorManager
            .GetIdentityBasedAsyncEnumerable(identity,
                query => query
                   .Select(entity => new IdentityInspectorResponse
                   {
                       Identity = entity.UniqueName,
                       UniqueName = entity.Inspector,
                       Activated = entity.Activated,
                       Organization = entity.Organization
                   }));
}