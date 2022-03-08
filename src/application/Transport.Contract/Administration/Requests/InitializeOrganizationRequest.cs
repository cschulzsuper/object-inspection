using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Requests
{
    public class InitializeOrganizationRequest
    {
        public string ETag { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string Inspector { get; set; } = string.Empty;
    }
}