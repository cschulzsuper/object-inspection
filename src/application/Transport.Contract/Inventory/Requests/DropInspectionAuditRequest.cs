using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Requests
{
    public class DropInspectionAuditRequest
    {
        [DayNumber]
        public int PlannedAuditDate { get; set; }
        
        [Milliseconds]
        public int PlannedAuditTime { get; set; }
    }
}