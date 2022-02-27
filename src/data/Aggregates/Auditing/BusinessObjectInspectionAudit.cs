namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionAudit
    {
        public int AuditTime { get; set; }
        public int AuditDate { get; set; }
        public string Inspector { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Annotation { get; set; } = string.Empty;
    }
}
