namespace Super.Paula.Application.Inventory.Responses
{
    public class BusinessObjectInspectionAuditScheduleAdjustmentResponse
    {
        public int PostponedAuditDate { get; set; }
        public int PostponedAuditTime { get; set; }

        public int PlannedAuditDate { get; set; }
        public int PlannedAuditTime { get; set; }
    }
}
