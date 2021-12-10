using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Guidelines.Events
{
    public class InspectionEvent
    {
        [StringLength(140)]
        public string? DisplayName { get; set; }

        [StringLength(4000)]
        public string? Text { get; set; }

        public bool? Activated { get; set; }
    }
}