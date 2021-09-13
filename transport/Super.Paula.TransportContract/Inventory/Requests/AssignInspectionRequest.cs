using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Inventory.Requests
{
    public class AssignInspectionRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        public string UniqueName { get; set; } = string.Empty;

        public bool Activated { get; set; } = false;
    }
}