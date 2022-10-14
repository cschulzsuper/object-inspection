namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspection
{
    public long Id { get; set; }
    public string ETag { get; set; } = string.Empty;

    public bool Activated { get; set; } = false;

    public string Inspection { get; set; } = string.Empty;
    public string InspectionDisplayName { get; set; } = string.Empty;
    public string InspectionText { get; set; } = string.Empty;

    public string BusinessObject { get; set; } = string.Empty;
    public string BusinessObjectDisplayName { get; set; } = string.Empty;

    public int AssignmentTime { get; set; }
    public int AssignmentDate { get; set; }

    public BusinessObjectInspectionAudit Audit { get; set; } = new BusinessObjectInspectionAudit();
    public BusinessObjectInspectionAuditSchedule AuditSchedule { get; set; } = new BusinessObjectInspectionAuditSchedule();
}