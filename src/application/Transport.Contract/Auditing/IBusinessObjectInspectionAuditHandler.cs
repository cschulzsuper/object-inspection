using System.Collections.Generic;
using System.Threading.Tasks;
using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditHandler
    {
        ValueTask<BusinessObjectInspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time);
        IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAll();
        IAsyncEnumerable<BusinessObjectInspectionAuditResponse> GetAllForBusinessObject(string businessObject);

        ValueTask<BusinessObjectInspectionAuditResponse> CreateAsync(string businessObject, BusinessObjectInspectionAuditRequest request);
        ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, BusinessObjectInspectionAuditRequest request);
        ValueTask DeleteAsync(string businessObject, string inspection, int date, int time);

        IAsyncEnumerable<BusinessObjectInspectionAuditResponse> Search(string? businessObject, string? inspector, string? inspection);
        IAsyncEnumerable<BusinessObjectInspectionAuditResponse> SearchForBusinessObject(string businessObject, string? inspector, string? inspection);

    }
}