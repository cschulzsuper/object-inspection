namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class CreateInspectionAuditRequest
    {
        public string Inspection { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int AuditDate { get; set; }
        public int AuditTime { get; set; }
    }
}