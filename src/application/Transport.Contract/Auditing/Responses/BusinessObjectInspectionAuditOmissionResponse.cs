using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Auditing.Responses
{
    public class BusinessObjectInspectionAuditOmissionResponse
    {
        public string ETag { get; set; } = string.Empty;
        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Appointments { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();

    }
}