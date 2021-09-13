using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Inventory.Requests
{
    public class CancelInspectionRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;
    }
}