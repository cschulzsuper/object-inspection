using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public class OrganizationRequestHandler : IOrganizationRequestHandler
{
    private readonly IOrganizationManager _organizationManager;
    private readonly IIdentityInspectorManager _identityInspectorManager;
    private readonly IOrganizationEventService _organizationEventService;
    private readonly IOrganizationContinuationService _organizationContinuationService;

    public OrganizationRequestHandler(
        IOrganizationManager organizationManager,
        IIdentityInspectorManager identityInspectorManager,
        IOrganizationEventService organizationEventService,
        IOrganizationContinuationService organizationContinuationService)
    {
        _organizationManager = organizationManager;
        _identityInspectorManager = identityInspectorManager;
        _organizationEventService = organizationEventService;
        _organizationContinuationService = organizationContinuationService;
    }

    public async ValueTask<OrganizationResponse> CreateAsync(OrganizationRequest request)
    {
        var entity = new Organization
        {
            ChiefInspector = request.ChiefInspector,
            DisplayName = request.DisplayName,
            UniqueName = request.UniqueName,
            Activated = request.Activated
        };

        await _organizationManager.InsertAsync(entity);

        await _organizationEventService.CreateOrganizationCreationEventAsync(entity);

        return new OrganizationResponse
        {
            ChiefInspector = entity.ChiefInspector,
            DisplayName = entity.DisplayName,
            UniqueName = entity.UniqueName,
            Activated = entity.Activated,
            ETag = entity.ETag
        };
    }

    public async ValueTask DeleteAsync(string organization, string etag)
    {
        var entity = await _organizationManager.GetAsync(organization);

        entity.ETag =etag;

        await _organizationManager.DeleteAsync(entity);

        var identityInspectors = _identityInspectorManager.GetQueryable()
            .Where(x => x.Organization == organization)
            .ToList();

        foreach (var identityInspector in identityInspectors)
        {
            await _identityInspectorManager.DeleteAsync(identityInspector);
        }

        await _organizationEventService.CreateOrganizationDeletionEventAsync(organization);
    }

    public IAsyncEnumerable<OrganizationResponse> GetAll()
        => _organizationManager
            .GetAsyncEnumerable(query => query
                .Select(entity => new OrganizationResponse
                {
                    ChiefInspector = entity.ChiefInspector,
                    DisplayName = entity.DisplayName,
                    UniqueName = entity.UniqueName,
                    Activated = entity.Activated,
                    ETag = entity.ETag
                }));

    public async ValueTask<OrganizationResponse> GetAsync(string organization)
    {
        var entity = await _organizationManager.GetAsync(organization);

        return new OrganizationResponse
        {
            ChiefInspector = entity.ChiefInspector,
            DisplayName = entity.DisplayName,
            UniqueName = entity.UniqueName,
            Activated = entity.Activated,
            ETag = entity.ETag
        };
    }

    public async ValueTask ReplaceAsync(string organization, OrganizationRequest request)
    {
        var entity = await _organizationManager.GetAsync(organization);

        var required =
            entity.Activated != request.Activated ||
            entity.DisplayName != request.DisplayName ||
            entity.ChiefInspector != request.ChiefInspector ||
            entity.UniqueName != request.UniqueName;

        if (required)
        {
            entity.Activated = request.Activated;
            entity.DisplayName = request.DisplayName;
            entity.ChiefInspector = request.ChiefInspector;
            entity.UniqueName = request.UniqueName;
            entity.ETag = request.ETag;

            await _organizationManager.UpdateAsync(entity);
            await _organizationEventService.CreateOrganizationUpdateEventAsync(entity);
        }
    }

    public async ValueTask<ActivateOrganizationResponse> ActivateAsync(string organization, string etag)
    {
        var entity = await _organizationManager.GetAsync(organization);

        var required = !entity.Activated;
        if (required)
        {
            entity.Activated = true;
            entity.ETag = etag;

            await _organizationManager.UpdateAsync(entity);
            await _organizationEventService.CreateOrganizationUpdateEventAsync(entity);
        }

        return new ActivateOrganizationResponse
        {
            ETag = entity.ETag
        };
    }

    public async ValueTask<DeactivateOrganizationResponse> DeactivateAsync(string organization, string etag)
    {
        var entity = await _organizationManager.GetAsync(organization);

        var required = entity.Activated;
        if (required)
        {
            entity.Activated = false;
            entity.ETag = etag;

            await _organizationManager.UpdateAsync(entity);
            await _organizationEventService.CreateOrganizationUpdateEventAsync(entity);
        }

        return new DeactivateOrganizationResponse
        {
            ETag = entity.ETag
        };

    }

    public async ValueTask<OrganizationResponse> RegisterAsync(RegisterOrganizationRequest request)
    {
        var entity = new Organization
        {
            ChiefInspector = string.Empty,
            UniqueName = request.UniqueName,
            DisplayName = request.DisplayName,
            Activated = false
        };

        await _organizationManager.InsertAsync(entity);
        await _organizationEventService.CreateOrganizationCreationEventAsync(entity);

        return new OrganizationResponse
        {
            ChiefInspector = entity.ChiefInspector,
            DisplayName = entity.DisplayName,
            UniqueName = entity.UniqueName,
            Activated = entity.Activated,
            ETag = entity.ETag
        };
    }

    public async ValueTask<InitializeOrganizationResponse> InitializeAsync(string organization, InitializeOrganizationRequest request)
    {
        var entity = await _organizationManager.GetAsync(organization);

        entity.ChiefInspector =  request.Inspector;
        entity.ETag =  request.ETag;
        entity.Activated = true;

        await _organizationManager.UpdateAsync(entity);
        await _organizationContinuationService.AddCreateInspectorContinuationForChiefInspectorAsync(entity);

        return new InitializeOrganizationResponse
        {
            ETag = entity.ETag
        };
    }
}