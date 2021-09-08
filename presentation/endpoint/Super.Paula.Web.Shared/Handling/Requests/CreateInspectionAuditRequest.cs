using Super.Paula.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class CreateInspectionAuditRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        public string Inspection { get; set; } = string.Empty;

        [Required]
        [StringRange("satisfying", "insufficient", "failed")]
        public string Result { get; set; } = string.Empty;
        public int AuditDate { get; set; }
        public int AuditTime { get; set; }
    }
}