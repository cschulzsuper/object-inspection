using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectHandler : IBusinessObjectHandler
    {
        private const string SearchTermKeyFreeText = "";
        private const string SearchTermKeyBusinessObject = "business-object";
        private const string SearchTermKeyInspector = "inspector";

        private readonly IBusinessObjectManager _businessObjectManager;
        private readonly IBusinessObjectEventService _businessObjectEventService;
 
        public BusinessObjectHandler(
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
                Inspector = entity.Inspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName,
                ETag = entity.ETag,
            };

            foreach (var extensionItem in entity)
            {
                response[extensionItem.Key] = extensionItem.Value;
            }

            return response;
        }

        public async IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var businessObjects = _businessObjectManager
                        .GetAsyncEnumerable(queryable => WhereSearchQuery(queryable, query)
                            .Skip(skip)
                            .Take(take));

            await foreach(var businessObject in businessObjects
                .WithCancellation(cancellationToken))
            {
                var response = new BusinessObjectResponse
                {
                    Inspector = businessObject.Inspector,
                    DisplayName = businessObject.DisplayName,
                    UniqueName = businessObject.UniqueName,
                    ETag = businessObject.ETag
                };

                foreach(var extensionItem in businessObject)
                {
                    response[extensionItem.Key] = extensionItem.Value;
                }

                yield return response;
            }
        }

        public async ValueTask<SearchBusinessObjectResponse> SearchAsync(string query)
        {
            await ValueTask.CompletedTask;

            var queryable = _businessObjectManager.GetQueryable();
            queryable = WhereSearchQuery(queryable, query);

            var topResults = queryable.Take(50);

            var response = new SearchBusinessObjectResponse
            {
                TotalCount = queryable.Count(),
                TopResults = new HashSet<BusinessObjectResponse>()
            };

            foreach (var topResult in topResults)
            {
                var responseItem = new BusinessObjectResponse
                {
                    Inspector = topResult.Inspector,
                    DisplayName = topResult.DisplayName,
                    UniqueName = topResult.UniqueName,
                    ETag = topResult.ETag
                };

                foreach (var extensionItem in topResult)
                {
                    responseItem[extensionItem.Key] = extensionItem.Value;
                }

                response.TopResults.Add(responseItem);
            }

            return response;
        }

        public async ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request)
        {
            var entity = new BusinessObject
            {
                Inspector = request.Inspector,
                DisplayName = request.DisplayName,
                UniqueName = request.UniqueName,
            };

            foreach (var extensionItem in request)
            {
                entity[extensionItem.Key] = extensionItem.Value;
            }

            await _businessObjectManager.InsertAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectEventAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, entity.Inspector, string.Empty);

            var response = new BusinessObjectResponse
            {
                Inspector = entity.Inspector,
                DisplayName = entity.DisplayName,
                UniqueName = entity.UniqueName,
                ETag = entity.ETag
            };

            foreach (var extensionItem in entity)
            {
                response[extensionItem.Key] = extensionItem.Value;
            }

            return response;
        }

        public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
        {
            var entity = await _businessObjectManager.GetAsync(businessObject);

            var oldInspector = entity.Inspector;

            entity.Inspector = request.Inspector;
            entity.DisplayName = request.DisplayName;
            entity.UniqueName = request.UniqueName;
            entity.ETag = request.ETag;

            foreach (var extensionItem in request)
            {
                entity[extensionItem.Key] = extensionItem.Value;
            }

            await _businessObjectManager.UpdateAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectEventAsync(entity);
            await _businessObjectEventService.CreateBusinessObjectInspectorEventAsync(entity, entity.Inspector, oldInspector);
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
                query = query.Where(x => x.DisplayName.Contains(freeText));
            }

            return query.OrderByDescending(x => x.UniqueName);
        }
    }
}