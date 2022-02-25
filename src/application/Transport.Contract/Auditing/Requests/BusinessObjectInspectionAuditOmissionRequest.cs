using Super.Paula.Validation;

namespace Super.Paula.Application.Auditing.Requests
{
    public class BusinessObjectInspectionAuditOmissionRequest
    {
        [DayNumber]
        public int PlannedAuditDate { get; set; }

        [Milliseconds]
        public int PlannedAuditTime { get; set; }
    }
}