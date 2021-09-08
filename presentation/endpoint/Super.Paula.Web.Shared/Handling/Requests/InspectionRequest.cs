using Super.Paula.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class InspectionRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;

        public bool Activated { get; set; }

        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(4000)]
        public string Text { get; set; } = string.Empty;

    }
}