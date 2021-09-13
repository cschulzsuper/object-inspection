using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Inventory.Requests
{
    public class ChangeInspectionAuditRequest
    {
        [Required]
        [ValidValues("satisfying", "insufficient", "failed")]
        public string Result { get; set; } = string.Empty;
    }
}