using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Web.Shared.Handling
{
    public interface IBusinessObjectInspectionAuditHandler
    {
        ValueTask<InspectionAuditResponse> GetAsync(string businessObject, string inspection, int date, int time);
        IAsyncEnumerable<InspectionAuditResponse> GetAll();
        IAsyncEnumerable<InspectionAuditResponse> GetAllForBusinessObject(string businessObject);

        ValueTask<InspectionAuditResponse> CreateAsync(string businessObject, InspectionAuditRequest request);
        ValueTask ReplaceAsync(string businessObject, string inspection, int date, int time, InspectionAuditRequest request);
        ValueTask DeleteAsync(string businessObject, string inspection, int date, int time);

        IAsyncEnumerable<InspectionAuditResponse> Search(string? businessObject, string? inspector, string? inspection);
        IAsyncEnumerable<InspectionAuditResponse> SearchForBusinessObject(string businessObject, string? inspector, string? inspection);

    }
}