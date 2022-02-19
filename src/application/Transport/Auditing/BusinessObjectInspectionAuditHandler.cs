using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionAuditHandler : IBusinessObjectInspectionAuditHandler
    {
        private const string SearchTermKeyFreeText = "";
        private const string SearchTermKeyBusinessObject = "business-object";
        private const string SearchTermKeyInspector = "inspector";
        private const string SearchTermKeyInspection = "inspection";
        private const string SearchTermKeyResult = "result";
        private const string SearchTermKeyFrom = "from";
        private const string SearchTermKeyTo = "to";

        private readonly IBusinessObjectInspectionAuditManager _businessObjectInspectionAuditManager;

        public BusinessObjectInspectionAuditHandler(IBusinessObjectInspectionAuditManager businessObjectInspectionAuditManager)
        {
            _businessObjectInspectionAuditManager = businessObjectInspectionAuditManager;
        }

        public async ValueTask<BusinessObjectInspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time)
        {
            var entity = await _businessObjectInspectionAuditManager.GetAsync(businessObject, inspection, date, time);

            return new BusinessObjectInspectionAuditResponse
            {
                Annotation = entity.Annotation,
                AuditDate = entity.AuditDate,
                AuditTime = entity.AuditTime,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                Inspector = entity.Inspector,
                Result = entity.Result
            };
        }

        public IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default)
            => _businessObjectInspectionAuditManager
                .GetAsyncEnumerable(queryable => WhereSearchQuery(queryable, query)
                    .Skip(skip)
                    .Take(take)
                    .Select(entity => new BusinessObjectInspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector,
                        Result = entity.Result
                    }), cancellationToken);

        public IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAllForBusinessObject(string businessObject, int skip, int take)
            => _businessObjectInspectionAuditManager
                .GetAsyncEnumerable(query => query
                    .Where(x => x.BusinessObject == businessObject)
                    .Skip(skip)
                    .Take(take)
                    .Select(entity => new BusinessObjectInspectionAuditResponse
                    {
                        Annotation = entity.Annotation,
                        AuditDate = entity.AuditDate,
                        AuditTime = entity.AuditTime,
                        BusinessObject = entity.BusinessObject,
                        BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                        Inspection = entity.Inspection,
                        InspectionDisplayName = entity.InspectionDisplayName,
                        Inspector = entity.Inspector,
                        Result = entity.Result
                    }));

        public async ValueTask<BusinessObjectInspectionAuditResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRequest request)
        {
            var entity = new BusinessObjectInspectionAudit
            {
                Annotation = request.Annotation,
                AuditDate = request.AuditDate,
                AuditTime = request.AuditTime,
                BusinessObject = businessObject,
                BusinessObjectDisplayName = request.BusinessObjectDisplayName,
                Inspection = request.Inspection,
                InspectionDisplayName = request.InspectionDisplayName,
                Inspector = request.Inspector,
                Result = request.Result
            };

            await _businessObjectInspectionAuditManager.InsertAsync(entity);

            return new BusinessObjectInspectionAuditResponse
            {
                Annotation = entity.Annotation,
                AuditDate = entity.AuditDate,
                AuditTime = entity.AuditTime,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                Inspector = entity.Inspector,
                Result = entity.Result
            };
        }

        public async ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request)
        {
            var entity = await _businessObjectInspectionAuditManager.GetAsync(businessObject, inspection, date, time);

            entity.Annotation = request.Annotation;
            entity.AuditDate = request.AuditDate;
            entity.AuditTime = request.AuditTime;
            entity.BusinessObject = businessObject;
            entity.BusinessObjectDisplayName = request.BusinessObjectDisplayName;
            entity.Inspection = request.Inspection;
            entity.InspectionDisplayName = request.InspectionDisplayName;
            entity.Inspector = request.Inspector;
            entity.Result = request.Result;

            await _businessObjectInspectionAuditManager.UpdateAsync(entity);
        }

        public async ValueTask DeleteAsync(string businessObject, string inspection, int date, int time)
        {
            var entity = await _businessObjectInspectionAuditManager.GetAsync(businessObject, inspection, date, time);

            await _businessObjectInspectionAuditManager.DeleteAsync(entity);
        }

        public async ValueTask<SearchBusinessObjectInspectionAuditResponse> SearchAsync(string query)
        {
            await ValueTask.CompletedTask;

            var queryable = _businessObjectInspectionAuditManager.GetQueryable();
            queryable = WhereSearchQuery(queryable, query);

            var topResult = queryable.Take(50)
                .Select(entity => new BusinessObjectInspectionAuditResponse
                {
                    Annotation = entity.Annotation,
                    AuditDate = entity.AuditDate,
                    AuditTime = entity.AuditTime,
                    BusinessObject = entity.BusinessObject,
                    BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                    Inspection = entity.Inspection,
                    InspectionDisplayName = entity.InspectionDisplayName,
                    Inspector = entity.Inspector,
                    Result = entity.Result
                })
                .ToHashSet();

            return new SearchBusinessObjectInspectionAuditResponse
            {
                TotalCount = queryable.Count(),
                TopResults = topResult
            };
        }

        private static IQueryable<BusinessObjectInspectionAudit> WhereSearchQuery(IQueryable<BusinessObjectInspectionAudit> query, string searchQuery)
        {
            var searchTerms = SearchQueryParser.Parse(searchQuery);
            var businessObjects = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyBusinessObject);
            var inspections = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyInspection);
            var inspectors = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyInspector);
            var results = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyResult);
            var from = searchTerms.GetValidSearchTermValues<int>(SearchTermKeyFrom).DefaultIfEmpty(default).Max();
            var to = searchTerms.GetValidSearchTermValues<int>(SearchTermKeyTo).DefaultIfEmpty(default).Max();
            var freeTexts = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyFreeText).Where(x => x.Length > 3).ToArray();

            query = query
                 .Where(x => !businessObjects.Any() || businessObjects.Contains(x.BusinessObject))
                 .Where(x => !inspections.Any() || inspections.Contains(x.Inspection))
                 .Where(x => !inspectors.Any() || inspectors.Contains(x.Inspector))
                 .Where(x => !results.Any() || results.Contains(x.Result))
                 .Where(x => from == default || x.AuditDate >= from)
                 .Where(x => to == default || x.AuditDate <= to);

            foreach (var freeText in freeTexts)
            {
                query = query.Where(x => x.BusinessObjectDisplayName.Contains(freeText) || x.InspectionDisplayName.Contains(freeText));
            }

            return query.OrderByDescending(x => x.AuditDate);
        }
    }
}