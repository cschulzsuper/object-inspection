using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Guidelines.Events
{
    public class InspectionEvent
    {
        [Required]
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(4000)]
        public string Text { get; set; } = string.Empty;

        public bool Activated { get; set; }
    }
}