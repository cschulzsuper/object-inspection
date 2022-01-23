using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Inventory.Responses
{
    public class BusinessObjectInspectionAuditScheduleResponse
    {
        public ISet<BusinessObjectInspectionAuditScheduleExpressionResponse> Expressions { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleExpressionResponse>();
        public int Threshold { get; set; }

        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Omissions { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();
        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Additionals { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();
        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Appointments { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();
    }
}
