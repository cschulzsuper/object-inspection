using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Inventory.Responses
{
    public class DropInspectionAuditResponse
    {
        public string BusinessObject { get; set; } = string.Empty;
        public string Inspection { get; set; } = string.Empty;

        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Delays { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();
        public ISet<BusinessObjectInspectionAuditScheduleTimestampResponse> Appointments { get; set; } = ImmutableHashSet.Create<BusinessObjectInspectionAuditScheduleTimestampResponse>();

    }
}