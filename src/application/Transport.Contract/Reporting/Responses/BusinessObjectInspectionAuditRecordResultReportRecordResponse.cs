namespace ChristianSchulz.ObjectInspection.Application.Reporting.Responses
{
    public class BusinessObjectInspectionAuditRecordResultReportRecordResponse
    {
        public required int AuditDate { get; set; }
        public required string Result { get; set; }
        public required decimal Percentag { get; set; }
    }
}
