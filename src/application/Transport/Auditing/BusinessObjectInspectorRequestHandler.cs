using ChristianSchulz.ObjectInspection.Application.Auditing.Requests;
using ChristianSchulz.ObjectInspection.Application.Auditing.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspectorRequestHandler : IBusinessObjectInspectorRequestHandler
{
    private readonly IBusinessObjectInspectorManager _businessObjectInspectorManager;
    private readonly IBusinessObjectInspectorEventService _businessObjectInspectorEventService;

    public BusinessObjectInspectorRequestHandler(
        IBusinessObjectInspectorManager businessObjectInspectorManager,
        IBusinessObjectInspectorEventService businessObjectInspectorEventService)
    {
        _businessObjectInspectorManager = businessObjectInspectorManager;
        _businessObjectInspectorEventService = businessObjectInspectorEventService;
    }

    public async ValueTask<BusinessObjectInspectorResponse> GetAsync(string businessObject, string inspector)
    {
        var entity = await _businessObjectInspectorManager.GetAsync(businessObject, inspector);

        return new BusinessObjectInspectorResponse
        {
            Inspector = entity.Inspector,
            BusinessObject = entity.BusinessObject,
            BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
            ETag = entity.ETag
        };
    }

    public IAsyncEnumerable<BusinessObjectInspectorResponse> GetAllForBusinessObject(string businessObject)
        => _businessObjectInspectorManager
            .GetAsyncEnumerable(queryable => queryable
                .Where(x => x.BusinessObject == businessObject)
                .Select(entity => new BusinessObjectInspectorResponse
                {
                    Inspector = entity.Inspector,
                    BusinessObject = entity.BusinessObject,
                    BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
                    ETag = entity.ETag
                }));

    public async ValueTask<BusinessObjectInspectorResponse> CreateAsync(string businessObject, BusinessObjectInspectorRequest request)
    {
        var entity = new BusinessObjectInspector
        {
            BusinessObject = businessObject,
            BusinessObjectDisplayName = request.BusinessObjectDisplayName,
            Inspector = request.Inspector
        };

        await _businessObjectInspectorManager.InsertAsync(entity);

        await _businessObjectInspectorEventService.CreateBusinessObjectInspectorCreationEventAsync(entity);

        return new BusinessObjectInspectorResponse
        {
            Inspector = entity.Inspector,
            BusinessObject = entity.BusinessObject,
            BusinessObjectDisplayName = entity.BusinessObjectDisplayName,
            ETag = entity.ETag
        };
    }

    public async ValueTask ReplaceAsync(string businessObject, string inspector, BusinessObjectInspectorRequest request)
    {
        var entity = await _businessObjectInspectorManager.GetAsync(businessObject, inspector);

        entity.BusinessObjectDisplayName = request.BusinessObjectDisplayName;
        entity.Inspector = request.Inspector;
        entity.ETag = request.ETag;

        await _businessObjectInspectorManager.UpdateAsync(entity);
    }

    public async ValueTask DeleteAsync(string businessObject, string inspector, string etag)
    {
        var entity = await _businessObjectInspectorManager.GetAsync(businessObject, inspector);

        entity.ETag = etag;

        await _businessObjectInspectorManager.DeleteAsync(entity);

        await _businessObjectInspectorEventService.CreateBusinessObjectInspectorDeletionEventAsync(entity);
    }
}