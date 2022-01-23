using System;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectInspectionAuditScheduleTimestamp
    {
        // HINT: https://github.com/dotnet/efcore/issues/24828
        public Guid Id { get; set; } = Guid.NewGuid();

        public int PlannedAuditDate { get; set; }
        public int PlannedAuditTime { get; set; }
    }
}
