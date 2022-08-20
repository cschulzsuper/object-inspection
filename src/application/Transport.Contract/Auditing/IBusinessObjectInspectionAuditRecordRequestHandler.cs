using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionAuditRecordRequestHandler
{
    ValueTask<BusinessObjectInspectionAuditRecordResponse> GetAsync(string businessObject, string inspection, int date, int time);
    IAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default);
    IAsyncEnumerable<BusinessObjectInspectionAuditRecordResponse> GetAllForBusinessObject(string businessObject, int skip, int take);

    ValueTask<BusinessObjectInspectionAuditRecordResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRecordRequest request);
    ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRecordRequest request);
    ValueTask DeleteAsync(string businessObject, string inspection, int date, int time, string etag);

    ValueTask<SearchBusinessObjectInspectionAuditRecordResponse> SearchAsync(string query);
}