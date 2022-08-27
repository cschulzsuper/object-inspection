using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using Super.Paula.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory;

public class BusinessObjectRequestHandler : IBusinessObjectRequestHandler
{
    private const string SearchTermKeyFreeText = "";
    private const string SearchTermKeyBusinessObject = "business-object";
    private const string SearchTermKeyInspector = "inspector";

    private readonly IBusinessObjectManager _businessObjectManager;
    private readonly IBusinessObjectEventService _businessObjectEventService;

    public BusinessObjectRequestHandler(
        IBusinessObjectManager businessObjectManager,
        IBusinessObjectEventService businessObjectEventService)
    {
        _businessObjectManager = businessObjectManager;
        _businessObjectEventService = businessObjectEventService;
    }

    public async ValueTask<BusinessObjectResponse> GetAsync(string businessObject)
    {
        var entity = await _businessObjectManager.GetAsync(businessObject);

        return new BusinessObjectResponse
        {
            Inspector = entity.Inspector,
            DisplayName = entity.DisplayName,
            UniqueName = entity.UniqueName,
            ETag = entity.ETag
        };
    }

    public IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default)
        => _businessObjectManager
            .GetAsyncEnumerable(queryable => WhereSearchQuery(queryable, query)
                .Skip(skip)
                .Take(take)
                .Select(entity => new BusinessObjectResponse
                {
                    Inspector = entity.Inspector,
                    DisplayName = entity.DisplayName,
                    UniqueName = entity.UniqueName,
                    ETag = entity.ETag
                }));

    public async ValueTask<SearchBusinessObjectResponse> SearchAsync(string query)
    {
        await ValueTask.CompletedTask;

        var queryable = _businessObjectManager.GetQueryable();
        queryable = WhereSearchQuery(queryable, query);

        var topResult = queryable.Take(50)
            .Select(entity => new BusinessObjectResponse
            {
                Inspector = entity.Inspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName,
                ETag = entity.ETag
            })
            .ToHashSet();

        return new SearchBusinessObjectResponse
        {
            TotalCount = queryable.Count(),
            TopResults = topResult
        };
    }

    public async ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request)
    {
        var entity = new BusinessObject
        {
            Inspector = request.Inspector,
            DisplayName = request.DisplayName,
            UniqueName = request.UniqueName,
        };

        await _businessObjectManager.InsertAsync(entity);
        await _businessObjectEventService.CreateBusinessObjectEventAsync(entity);
        await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, entity.Inspector, string.Empty);

        return new BusinessObjectResponse
        {
            Inspector = entity.Inspector,
            DisplayName = entity.DisplayName,
            UniqueName = entity.UniqueName,
            ETag = entity.ETag
        };
    }

    public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
    {
        var entity = await _businessObjectManager.GetAsync(businessObject);

        var oldInspector = entity.Inspector;

        var required =
            entity.DisplayName != request.DisplayName ||
            entity.Inspector != request.Inspector;

        if (required)
        {
            entity.Inspector = request.Inspector;
            entity.DisplayName = request.DisplayName;
            entity.UniqueName = request.UniqueName;
            entity.ETag = request.ETag;

            await _businessObjectManager.UpdateAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectEventAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, entity.Inspector, oldInspector);
        }
    }

    public async ValueTask DeleteAsync(string businessObject, string etag)
    {
        var entity = await _businessObjectManager.GetAsync(businessObject);

        entity.ETag = etag;

        await _businessObjectManager.DeleteAsync(entity);
        await _businessObjectEventService.CreateBusinessObjectDeletionEventAsync(businessObject);
        await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, string.Empty, entity.Inspector);
    }

    private static IQueryable<BusinessObject> WhereSearchQuery(IQueryable<BusinessObject> query, string searchQuery)
    {
        var searchTerms = SearchQueryParser.Parse(searchQuery);
        var businessObjects = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyBusinessObject);
        var inspectors = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyInspector);
        var freeTexts = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyFreeText).Where(x => x.Length > 3).ToArray();

        query = query
             .Where(x => !businessObjects.Any() || businessObjects.Contains(x.UniqueName))
             .Where(x => !inspectors.Any() || inspectors.Contains(x.Inspector));

        foreach (var freeText in freeTexts)
        {
            query = query.Where(x => 
                x.DisplayName.Contains(freeText) ||
                x.Inspector.Contains(freeText) ||
                x.UniqueName.Contains(freeText));
        }

        return query.OrderByDescending(x => x.UniqueName);
    }
}