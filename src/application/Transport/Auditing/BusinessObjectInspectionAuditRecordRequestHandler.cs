using ChristianSchulz.ObjectInspection.Application.Auditing.Requests;
using ChristianSchulz.ObjectInspection.Application.Auditing.Responses;
using ChristianSchulz.ObjectInspection.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspectionAuditRecordRequestHandler : IBusinessObjectInspectionAuditRecordRequestHandler
{
    private const string SearchTermKeyFreeText = "";
    private const string SearchTermKeyBusinessObject = "business-object";
    private const string SearchTermKeyInspector = "inspector";
    private const string SearchTermKeyInspection = "inspection";
    private const string SearchTermKeyResult = "result";
    private const string SearchTermKeyFromDate = "from-date";
    private const string SearchTermKeyFromTime = "from-time";
    private const string SearchTermKeyToDate = "to-date";
    private const string SearchTermKeyToTime = "to-time";

    private readonly IBusinessObjectInspectionAuditRecordManager _businessObjectInspectionAuditRecordManager;

    public BusinessObjectInspectionAuditRecordRequestHandler(IBusinessObjectInspectionAuditRecordManager businessObjectInspectionAuditRecordManager)
    {
        _businessObjectInspectionAuditRecordManager = businessObjectInspectionAuditRecordManager;
    }

    public async ValueTask<BusinessObjectInspectionAuditRecordResponse> GetAsync(string businessObject, string inspection, int date, int time)
    {
        var entity = await _businessObjectInspectionAuditRecordManager.GetAsync(businessObject, inspection, date, time);

        return new BusinessObjectInspectionAuditRecordResponse
        {
            Annotation = entity.Annotation,
            AuditDate = entity.AuditDate,
            AuditTime = entity.AuditTime,
            BusinessObject = entity.BusinessObject,
            BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
            Inspection = entity.Inspection,
            InspectionDisplayName = entity.InspectionDisplayName,
            Inspector = entity.Inspector,
            Result = entity.Result,
            ETag = entity.ETag
        };
    }

    public IAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default)
        => _businessObjectInspectionAuditRecordManager
            .GetAsyncEnumerable(queryable => WhereSearchQuery(queryable, query)
                .Skip(skip)
                .Take(take)
                .Select(entity => new BusinessObjectInspectionAuditRecordResponse
                {
                    Annotation = entity.Annotation,
                    AuditDate = entity.AuditDate,
                    AuditTime = entity.AuditTime,
                    BusinessObject = entity.BusinessObject,
                    BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                    Inspection = entity.Inspection,
                    InspectionDisplayName = entity.InspectionDisplayName,
                    Inspector = entity.Inspector,
                    Result = entity.Result,
                    ETag = entity.ETag
                }), cancellationToken);

    public IAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse> GetAllForBusinessObject(string businessObject, int skip, int take)
        => _businessObjectInspectionAuditRecordManager
            .GetAsyncEnumerable(query => query
                .Where(x => x.BusinessObject == businessObject)
                .Skip(skip)
                .Take(take)
                .Select(entity => new BusinessObjectInspectionAuditRecordResponse
                {
                    Annotation = entity.Annotation,
                    AuditDate = entity.AuditDate,
                    AuditTime = entity.AuditTime,
                    BusinessObject = entity.BusinessObject,
                    BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                    Inspection = entity.Inspection,
                    InspectionDisplayName = entity.InspectionDisplayName,
                    Inspector = entity.Inspector,
                    Result = entity.Result,
                    ETag = entity.ETag
                }));

    public async ValueTask<BusinessObjectInspectionAuditRecordResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRecordRequest request)
    {
        var entity = new BusinessObjectInspectionAuditRecord
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

        await _businessObjectInspectionAuditRecordManager.InsertAsync(entity);

        return new BusinessObjectInspectionAuditRecordResponse
        {
            Annotation = entity.Annotation,
            AuditDate = entity.AuditDate,
            AuditTime = entity.AuditTime,
            BusinessObject = entity.BusinessObject,
            BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
            Inspection = entity.Inspection,
            InspectionDisplayName = entity.InspectionDisplayName,
            Inspector = entity.Inspector,
            Result = entity.Result,
            ETag = entity.ETag
        };
    }

    public async ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRecordRequest request)
    {
        var entity = await _businessObjectInspectionAuditRecordManager.GetAsync(businessObject, inspection, date, time);

        entity.Annotation = request.Annotation;
        entity.AuditDate = request.AuditDate;
        entity.AuditTime = request.AuditTime;
        entity.BusinessObject = businessObject;
        entity.BusinessObjectDisplayName = request.BusinessObjectDisplayName;
        entity.Inspection = request.Inspection;
        entity.InspectionDisplayName = request.InspectionDisplayName;
        entity.Inspector = request.Inspector;
        entity.Result = request.Result;
        entity.ETag = request.ETag;

        await _businessObjectInspectionAuditRecordManager.UpdateAsync(entity);
    }

    public async ValueTask DeleteAsync(string businessObject, string inspection, int date, int time, string etag)
    {
        var entity = await _businessObjectInspectionAuditRecordManager.GetAsync(businessObject, inspection, date, time);

        entity.ETag = etag;

        await _businessObjectInspectionAuditRecordManager.DeleteAsync(entity);
    }

    public async ValueTask<SearchBusinessObjectInspectionAuditRecordResponse> SearchAsync(string query)
    {
        await ValueTask.CompletedTask;

        var queryable = _businessObjectInspectionAuditRecordManager.GetQueryable();
        queryable = WhereSearchQuery(queryable, query);

        var topResult = queryable.Take(50)
            .Select(entity => new BusinessObjectInspectionAuditRecordResponse
            {
                Annotation = entity.Annotation,
                AuditDate = entity.AuditDate,
                AuditTime = entity.AuditTime,
                BusinessObject = entity.BusinessObject,
                BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                Inspection = entity.Inspection,
                InspectionDisplayName = entity.InspectionDisplayName,
                Inspector = entity.Inspector,
                Result = entity.Result,
                ETag = entity.ETag
            })
            .ToHashSet();

        return new SearchBusinessObjectInspectionAuditRecordResponse
        {
            TotalCount = queryable.Count(),
            TopResults = topResult
        };
    }

    private static IQueryable<BusinessObjectInspectionAuditRecord> WhereSearchQuery(IQueryable<BusinessObjectInspectionAuditRecord> query, string searchQuery)
    {
        var searchTerms = SearchQueryParser.Parse(searchQuery);
        var businessObjects = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyBusinessObject);
        var inspections = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyInspection);
        var inspectors = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyInspector);
        var results = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyResult);
        var fromDate = searchTerms.GetValidSearchTermValues<int>(SearchTermKeyFromDate).DefaultIfEmpty(default).Max();
        var fromTime = searchTerms.GetValidSearchTermValues<int>(SearchTermKeyFromTime).DefaultIfEmpty(default).Max();
        var toDate = searchTerms.GetValidSearchTermValues<int>(SearchTermKeyToDate).DefaultIfEmpty(default).Max();
        var toTime = searchTerms.GetValidSearchTermValues<int>(SearchTermKeyToTime).DefaultIfEmpty(default).Max();
        var freeTexts = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyFreeText).Where(x => x.Length > 3).ToArray();

        query = query
             .Where(x => !businessObjects.Any() || businessObjects.Contains(x.BusinessObject))
             .Where(x => !inspections.Any() || inspections.Contains(x.Inspection))
             .Where(x => !inspectors.Any() || inspectors.Contains(x.Inspector))
             .Where(x => !results.Any() || results.Contains(x.Result))
             .Where(x => (fromDate == default && fromTime == default) || (x.AuditDate == fromDate && x.AuditTime >= fromTime) || (x.AuditDate > fromDate))
             .Where(x => (toDate == default && toTime == default) || (x.AuditDate == toDate && x.AuditTime <= toTime) || (x.AuditDate < toDate));

        foreach (var freeText in freeTexts)
        {
            query = query.Where(x => x.BusinessObjectDisplayName.Contains(freeText) || x.InspectionDisplayName.Contains(freeText));
        }

        return query.OrderByDescending(x => x.AuditDate);
    }
}