using Super.Paula.Validation;

namespace Super.Paula.Application.Auditing.Requests
{
    public class BusinessObjectInspectionAuditRequest
    {
        public string ETag { get; set; } = string.Empty;

        [AuditResult]
        public string Result { get; set; } = string.Empty;
        public int RequestDate { get; set; }
        public int RequestTime { get; set; }
    }
}