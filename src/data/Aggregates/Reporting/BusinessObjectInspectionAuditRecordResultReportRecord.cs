namespace ChristianSchulz.ObjectInspection.Application.Reporting;

public class BusinessObjectInspectionAuditRecordResultReportRecord
{
    public required int AuditDate { get; set; }
    public required string Result { get; set; }
    public required decimal Percentag { get; set; }
}

