using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Requests
{
    public class ScheduleInspectionRequest
    {
        [CronExpression(AllowEmptyStrings = true)]
        public string AuditSchedule { get; set; } = string.Empty;

        [Milliseconds]
        public int AuditDelayThreshold { get; set; }

        [Milliseconds]
        public int AuditThreshold { get; set; }
    }
}