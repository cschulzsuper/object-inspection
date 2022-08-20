namespace Super.Paula.Application.Administration.Responses;

public class InspectorBusinessObjectResponse
{
    public string UniqueName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public bool AuditSchedulePending { get; set; }
    public bool AuditScheduleDelayed { get; set; }
    public int AuditSchedulePlannedAuditDate { get; set; }
    public int AuditSchedulePlannedAuditTime { get; set; }
}