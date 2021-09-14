using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;

namespace Super.Paula.Application.Inventory
{
    public interface IBusinessObjectHandler
    {
        ValueTask<BusinessObjectResponse> GetAsync(string businessObject);
        
        IAsyncEnumerable<BusinessObjectResponse> GetAll();
        IAsyncEnumerable<BusinessObjectResponse> GetAllForInspector(string inspector);
        IAsyncEnumerable<BusinessObjectResponse> Search(string? businessObject, string? inspector);

        ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request);
        ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request);
        ValueTask DeleteAsync(string businessObject);

        ValueTask AssignInspectionAsync(string businessObject, AssignInspectionRequest request);
        ValueTask CancelInspectionAsync(string businessObject, CancelInspectionRequest request);
        ValueTask RefreshInspectionAsync(string inspection, RefreshInspectionRequest request);

        ValueTask CreateInspectionAuditAsync(string businessObject, CreateInspectionAuditRequest request);
        ValueTask ChangeInspectionAuditAsync(string businessObject, string inspection, ChangeInspectionAuditRequest request);
        ValueTask AnnotateInspectionAuditAsync(string businessObject, string inspection, AnnotateInspectionAuditRequest request);
    }
}