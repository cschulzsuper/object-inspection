namespace Super.Paula.Application.Administration
{
    public class InspectorBusinessObject
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public int AuditSchedulePlannedAuditDate { get; set; }
        public int AuditSchedulePlannedAuditTime { get; set; }

        public bool AuditSchedulePending { get; set; }
        public bool AuditScheduleDelayed { get; set; }
    }
}
