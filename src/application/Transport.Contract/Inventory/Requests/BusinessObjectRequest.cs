using System.ComponentModel.DataAnnotations;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Requests
{
    public class BusinessObjectRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        [InvalidValues("search")]
        public string UniqueName { get; set; } = string.Empty;

        [KebabCase]
        [StringLength(140)]
        public string Inspector { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

    }
}