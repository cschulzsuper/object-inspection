namespace Super.Paula.Aggregates.Auditing
{
    public class BusinessObjectInspectionAudit
    {
        public string BusinessObject { get; set; } = string.Empty;
        public string BusinessObjectDisplayName { get; set; } = string.Empty;
        public string Inspection { get; set; } = string.Empty;
        public string InspectionDisplayName { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;
        public int AuditDate { get; set; } = 0;
        public int AuditTime { get; set; } = 0;
        public string Result { get; set; } = string.Empty;
        public string Annotation { get; set; } = string.Empty;
    }
}
