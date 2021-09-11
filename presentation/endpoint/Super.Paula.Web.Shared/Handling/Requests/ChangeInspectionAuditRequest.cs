using Super.Paula.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class ChangeInspectionAuditRequest
    {
        [Required]
        [ValidValues("satisfying", "insufficient", "failed")]
        public string Result { get; set; } = string.Empty;
    }
}