using System.ComponentModel.DataAnnotations;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Events
{
    public class BusinessObjectInspectionAuditScheduleEvent
    {
        [KebabCase]
        [StringLength(140)]
        public string? Inspector { get; set; }

        [DayNumber]
        public int? PlannedAuditDate { get; set; }

        [Milliseconds]
        public int? PlannedAuditTime { get; set; }

        [Milliseconds]
        public int? Threshold { get; set; }
    }
}