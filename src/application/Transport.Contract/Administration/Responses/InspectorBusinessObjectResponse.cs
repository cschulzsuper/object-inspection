using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    public class InspectorBusinessObjectResponse
    {
        public string UniqueName { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public bool AuditSchedulePending { get; set; }

        public bool AuditScheduleDelayed { get; set; }
    }
}