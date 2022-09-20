namespace ChristianSchulz.ObjectInspection.Application.Auditing.Responses;

public class BusinessObjectInspectionResponse
{
    public string ETag { get; set; } = string.Empty;

    public bool Activated { get; set; } = false;

    public string Inspection { get; set; } = string.Empty;
    public string InspectionDisplayName { get; set; } = string.Empty;
    public string InspectionText { get; set; } = string.Empty;
    public string BusinessObject { get; set; } = string.Empty;
    public string BusinessObjectDisplayName { get; set; } = string.Empty;

    public int AuditDate { get; set; }
    public int AuditTime { get; set; }

    public string AuditInspector { get; set; } = string.Empty;
    public string AuditResult { get; set; } = string.Empty;
    public string AuditAnnotation { get; set; } = string.Empty;

    public BusinessObjectInspectionAuditScheduleResponse AuditSchedule { get; set; } = new BusinessObjectInspectionAuditScheduleResponse();
}