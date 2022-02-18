using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public interface IBusinessObjectHandler
    {
        ValueTask<BusinessObjectResponse> GetAsync(string businessObject);
        IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default);
        ValueTask<SearchBusinessObjectResponse> SearchAsync(string query);

        ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request);
        ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request);
        ValueTask DeleteAsync(string businessObject);

        ValueTask AssignInspectionAsync(string businessObject, AssignInspectionRequest request);
        ValueTask CancelInspectionAsync(string businessObject, CancelInspectionRequest request);
        ValueTask ScheduleInspectionAuditAsync(string businessObject, string inspection, ScheduleInspectionAuditRequest request);
        ValueTask TimeInspectionAuditAsync(string businessObject);
        ValueTask<DropInspectionAuditResponse> DropInspectionAuditAsync(string businessObject, string inspection, DropInspectionAuditRequest request);

        ValueTask<CreateInspectionAuditResponse> CreateInspectionAuditAsync(string businessObject, CreateInspectionAuditRequest request);
        ValueTask ChangeInspectionAuditAsync(string businessObject, string inspection, ChangeInspectionAuditRequest request);
        ValueTask AnnotateInspectionAuditAsync(string businessObject, string inspection, AnnotateInspectionAuditRequest request);
    }
}