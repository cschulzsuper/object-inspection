using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Requests
{
    public class PostponeInspectionAuditRequest
    {
        [CronExpression]
        public string Schedule { get; set; } = string.Empty;

        [DayNumber]
        public int PostponedAuditDate { get; set; }

        [Milliseconds]
        public int PostponedAuditTime { get; set; }

        [DayNumber]
        public int PlannedAuditDate { get; set; }
        
        [Milliseconds]
        public int PlannedAuditTime { get; set; }
    }
}