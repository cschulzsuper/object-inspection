using Super.Paula.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class OrganizationRequest
    {
        [KebabCase]
        [StringLength(140)]
        public string ChiefInspector { get; set; } = string.Empty;

        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;

        public bool Activated { get; set; }
    }
}