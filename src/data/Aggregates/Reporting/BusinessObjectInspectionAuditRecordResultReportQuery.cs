namespace ChristianSchulz.ObjectInspection.Application.Reporting;

public record BusinessObjectInspectionAuditRecordResultReportQuery
{
    public required string Inspector { get; set; }

    public required string Inspection { get; set; }

    public required string BusinessObject { get; set; }

    public required int FromDate { get; set; }

    public required int ToDate { get; set; }
}
