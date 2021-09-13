using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Administration.Requests
{
    public class StartImpersonationRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        public string Organization { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;
    }
}