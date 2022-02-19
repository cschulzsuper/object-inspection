using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditHandler
    {
        ValueTask<BusinessObjectInspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time);
        IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default);
        IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAllForBusinessObject(string businessObject, int skip, int take);

        ValueTask<BusinessObjectInspectionAuditResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRequest request);
        ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request);
        ValueTask DeleteAsync(string businessObject, string inspection, int date, int time);

        ValueTask<SearchBusinessObjectInspectionAuditResponse> SearchAsync(string query);
    }
}