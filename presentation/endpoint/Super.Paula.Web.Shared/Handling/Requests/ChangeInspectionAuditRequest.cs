using Super.Paula.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class ChangeInspectionAuditRequest
    {
        [Required]
        [StringRange("satisfying", "insufficient", "failed")]
        public string Result { get; set; } = string.Empty;
    }
}