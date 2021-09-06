﻿using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Shared.Handling
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
    }
}