using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    public class InspectorBusinessObjectEvent
    {
        [StringLength(140)]
        [UniqueName]
        public string? UniqueName { get; set; }

        [StringLength(140)]
        public string? DisplayName { get; set; }

        public bool? NewAuditSchedulePending { get; set; }

        public bool? OldAuditSchedulePending { get; set; }

        public bool? NewAuditScheduleDelayed { get; set; }

        public bool? OldAuditScheduleDelayed { get; set; }
    }
}