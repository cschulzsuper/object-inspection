using System.ComponentModel.DataAnnotations;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Requests
{
    public class CreateInspectionAuditRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string Inspection { get; set; } = string.Empty;

        [Required]
        [AuditResult]
        public string Result { get; set; } = string.Empty;
        public int AuditDate { get; set; }
        public int AuditTime { get; set; }
    }
}