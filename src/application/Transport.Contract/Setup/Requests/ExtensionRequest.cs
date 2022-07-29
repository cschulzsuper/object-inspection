using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Setup.Requests
{
    public class ExtensionRequest
    {
        public string ETag { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string UniqueName { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        [ExtensionType]
        public string Type { get; set; } = string.Empty;
    }
}