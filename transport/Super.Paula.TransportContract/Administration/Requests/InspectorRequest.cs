using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Administration.Requests
{
    public class InspectorRequest
    {
        [EmailAddress]
        [StringLength(140)]
        public string MailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string Secret { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;

        public bool Activated { get; set; } = false;
    }
}