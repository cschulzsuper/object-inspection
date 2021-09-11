using Super.Paula.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
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