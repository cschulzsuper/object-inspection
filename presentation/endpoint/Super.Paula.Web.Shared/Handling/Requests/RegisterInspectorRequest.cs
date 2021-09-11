using Super.Paula.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class RegisterInspectorRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        public string Organization { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(140)]
        public string MailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string Secret { get; set; } = string.Empty;
    }
}