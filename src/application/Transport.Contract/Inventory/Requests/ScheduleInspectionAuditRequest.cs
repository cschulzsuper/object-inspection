using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Requests
{
    public class ScheduleInspectionAuditRequest
    {
        [CronExpression]
        public string Schedule { get; set; } = string.Empty;

        [Milliseconds]
        public int DelayThreshold { get; set; }

        [Milliseconds]
        public int Threshold { get; set; }
    }
}