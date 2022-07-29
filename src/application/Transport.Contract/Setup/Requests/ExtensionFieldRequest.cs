using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Setup.Requests
{
    public class ExtensionFieldRequest
    {
        public string ETag { get; set; } = string.Empty;

        [Required]
        [CamelCase]
        [StringLength(140)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [LowerCase]
        [StringLength(140)]
        [ExtensionFieldType]
        public string Type { get; set; } = string.Empty;
    }
}