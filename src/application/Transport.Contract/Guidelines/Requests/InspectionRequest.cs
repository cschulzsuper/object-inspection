using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Guidelines.Requests
{
    public class InspectionRequest
    {
        public string ETag { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string UniqueName { get; set; } = string.Empty;

        public bool Activated { get; set; }

        [Required]
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(4000)]
        public string Text { get; set; } = string.Empty;

    }
}