using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    public class InspectorBusinessObjectEvent
    {
        [StringLength(140)]
        public string? BusinessObject { get; set; }

        [StringLength(140)]
        public string? BusinessObjectDisplayName { get; set; }

        public bool? NewPending { get; set; }

        public bool? OldPending { get; set; }

        public bool? NewDelayed { get; set; }

        public bool? OldDelayed { get; set; }
    }
}