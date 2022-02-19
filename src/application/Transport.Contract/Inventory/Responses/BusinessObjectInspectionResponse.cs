namespace Super.Paula.Application.Inventory.Responses
{
    public class BusinessObjectInspectionResponse
    {
        public bool Activated { get; set; } = false;

        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        public int AuditDate { get; set; }
        public int AuditTime { get; set; }

        public string AuditInspector { get; set; } = string.Empty;
        public string AuditResult { get; set; } = string.Empty;
        public string AuditAnnotation { get; set; } = string.Empty;

        public BusinessObjectInspectionAuditScheduleResponse AuditSchedule { get; set; } = new BusinessObjectInspectionAuditScheduleResponse();
    }
}