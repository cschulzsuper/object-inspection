using ChristianSchulz.ObjectInspection.Application.Auditing.Requests;
using ChristianSchulz.ObjectInspection.Application.Auditing.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectorRequestHandler
{
    ValueTask<BusinessObjectInspectorResponse> GetAsync(string businessObject, string inspector);
    IAsyncEnumerable<BusinessObjectInspectorResponse> GetAllForBusinessObject(string businessObject);

    ValueTask<BusinessObjectInspectorResponse> CreateAsync(string businessObject, BusinessObjectInspectorRequest request);
    ValueTask ReplaceAsync(string businessObject, string inspector, BusinessObjectInspectorRequest request);
    ValueTask DeleteAsync(string businessObject, string inspector, string etag);
}