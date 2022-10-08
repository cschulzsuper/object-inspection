using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Reporting;

public class BusinessObjectInspectionAuditRecordReport
{
    public required BusinessObjectInspectionAuditRecordResultReportQuery Query { get; set; }
    public required string QueryHash { get; set; }
    public required int ReportDate { get; set; }
    public required int ReportTime { get; set; }
    public required ISet<BusinessObjectInspectionAuditRecordResultReportRecord> Records { get; set; }
}