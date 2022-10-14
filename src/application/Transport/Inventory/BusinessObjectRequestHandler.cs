using ChristianSchulz.ObjectInspection.Application.Authentication;
using ChristianSchulz.ObjectInspection.Application.Inventory.Requests;
using ChristianSchulz.ObjectInspection.Application.Inventory.Responses;
using ChristianSchulz.ObjectInspection.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

public class BusinessObjectRequestHandler : IBusinessObjectRequestHandler
{
    private const string SearchTermKeyFreeText = "";
    private const string SearchTermKeyBusinessObject = "business-object";

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

        var response = new BusinessObjectResponse
        {
            DisplayName = entity.DisplayName,
            DistinctionType = entity.DistinctionType,
            UniqueName = entity.UniqueName,
            ETag = entity.ETag
            
        };

        foreach(var field in entity.ExtensionFields)
        {
            response[field.Key] = field.Value;
        }

        return response;
    }

    public async IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take, 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var entities = _businessObjectManager
                .GetAsyncEnumerable(queryable => WhereSearchQuery(queryable, query)
                    .Skip(skip)
                    .Take(take));

        await foreach(var entity in entities
            .WithCancellation(cancellationToken))
        {
            var response = new BusinessObjectResponse
            {
                DisplayName = entity.DisplayName,
                DistinctionType = entity.DistinctionType,
                UniqueName = entity.UniqueName,
                ETag = entity.ETag
            };

            foreach (var field in entity.ExtensionFields)
            {
                response[field.Key] = field.Value;
            }

            yield return response;
        }
    }

    public async ValueTask<SearchBusinessObjectResponse> SearchAsync(string query)
    {
        await ValueTask.CompletedTask;

        var queryable = _businessObjectManager.GetQueryable();
        queryable = WhereSearchQuery(queryable, query);

        var topResult = queryable.Take(50)
            .AsEnumerable()
            .Select(entity =>
            {
                var response = new BusinessObjectResponse
                {
                    DisplayName = entity.DisplayName,
                    DistinctionType = entity.DistinctionType,
                    UniqueName = entity.UniqueName,
                    ETag = entity.ETag
                };

                foreach (var field in entity.ExtensionFields)
                {
                    response[field.Key] = field.Value;
                }

                return response;
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
            DisplayName = request.DisplayName,
            DistinctionType = request.DistinctionType,
            UniqueName = request.UniqueName,
        };

        foreach (var field in request)
        {
            entity.ExtensionFields[field.Key] = field.Value;
        }

        await _businessObjectManager.InsertAsync(entity);

        var response = new BusinessObjectResponse
        {
            DisplayName = entity.DisplayName,
            DistinctionType = entity.DistinctionType,
            UniqueName = entity.UniqueName,
            ETag = entity.ETag
        };

        foreach (var field in entity.ExtensionFields)
        {
            response[field.Key] = field.Value;
        }

        return response;
    }

    public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
    {
        var entity = await _businessObjectManager.GetAsync(businessObject);

        entity.DisplayName = request.DisplayName;
        entity.DistinctionType = request.DistinctionType;
        entity.UniqueName = request.UniqueName;
        entity.ETag = request.ETag;

        foreach (var field in request)
        {
            entity.ExtensionFields[field.Key] = field.Value;
        }

        await _businessObjectManager.UpdateAsync(entity);
        await _businessObjectEventService.CreateBusinessObjectUpdateEventAsync(entity);
    }

    public async ValueTask DeleteAsync(string businessObject, string etag)
    {
        var entity = await _businessObjectManager.GetAsync(businessObject);

        entity.ETag = etag;

        await _businessObjectManager.DeleteAsync(entity);
        await _businessObjectEventService.CreateBusinessObjectDeletionEventAsync(businessObject);
    }

    private static IQueryable<BusinessObject> WhereSearchQuery(IQueryable<BusinessObject> query, string searchQuery)
    {
        var searchTerms = SearchQueryParser.Parse(searchQuery);
        var businessObjects = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyBusinessObject);
        var freeTexts = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyFreeText).Where(x => x.Length > 3).ToArray();

        query = query
             .Where(x => !businessObjects.Any() || businessObjects.Contains(x.UniqueName));

        foreach (var freeText in freeTexts)
        {
            query = query.Where(x => 
                x.DisplayName.Contains(freeText) ||
                x.UniqueName.Contains(freeText));
        }

        return query.OrderByDescending(x => x.UniqueName);
    }
}