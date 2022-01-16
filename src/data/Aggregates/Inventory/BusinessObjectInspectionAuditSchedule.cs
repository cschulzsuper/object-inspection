using System.Collections.Generic;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectInspectionAuditSchedule
    {
        public string CronExpression { get; set; } = string.Empty;

        public ISet<BusinessObjectInspectionAuditScheduleAdjustment> Adjustments { get; set; } = new HashSet<BusinessObjectInspectionAuditScheduleAdjustment>();
    }
}
