using Super.Paula.Application.Auditing.Requests;
using Super.Paula.Application.Auditing.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionRequestHandler
{
    ValueTask<BusinessObjectInspectionResponse> GetAsync(string businessObject, string inspection);
    IAsyncEnumerable<BusinessObjectInspectionResponse> GetAllForBusinessObject(string businessObject);

    ValueTask<BusinessObjectInspectionResponse> CreateAsync(string businessObject, BusinessObjectInspectionRequest request);
    ValueTask ReplaceAsync(string businessObject, string inspection, BusinessObjectInspectionRequest request);
    ValueTask DeleteAsync(string businessObject, string inspection, string etag);

    ValueTask<BusinessObjectInspectionAuditScheduleResponse> ReplaceAuditScheduleAsync(string businessObject, string inspection, BusinessObjectInspectionAuditScheduleRequest request);
    ValueTask RecalculateInspectionAuditAppointmentsAsync(string businessObject);

    ValueTask<BusinessObjectInspectionAuditOmissionResponse> CreateAuditOmissionAsync(string businessObject, string inspection, BusinessObjectInspectionAuditOmissionRequest request);

    ValueTask<BusinessObjectInspectionAuditResponse> CreateAuditAsync(string businessObject, string inspection, BusinessObjectInspectionAuditRequest request);
    ValueTask<BusinessObjectInspectionAuditResponse> ReplaceAuditAsync(string businessObject, string inspection, BusinessObjectInspectionAuditRequest request);
    ValueTask<BusinessObjectInspectionAuditAnnotationResponse> ReplaceAuditAnnotationAsync(string businessObject, string inspection, BusinessObjectInspectionAuditAnnotationRequest request);
}