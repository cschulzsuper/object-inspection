namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class InspectionAuditRequest
    {
        public string Annotation { get; set; } = string.Empty;
        public int AuditDate { get; set; }
        public int AuditTime { get; set; }
        public string BusinessObject { get; set; } = string.Empty;
        public string Inspection { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string BusinessObjectDisplayName { get; set; } = string.Empty;
        public string InspectionDisplayName { get; set; } = string.Empty;
    }
}