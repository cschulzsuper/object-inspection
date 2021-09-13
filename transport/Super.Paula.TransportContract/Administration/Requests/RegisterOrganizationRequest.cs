using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Administration.Requests
{
    public class RegisterOrganizationRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(140)]
        public string ChiefInspectorMail { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        public string ChiefInspector { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string ChiefInspectorSecret { get; set; } = string.Empty;
    }
}