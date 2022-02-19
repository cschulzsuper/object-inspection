using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Requests
{
    public class ChangeInspectionAuditRequest
    {
        [Required]
        [AuditResult]
        public string Result { get; set; } = string.Empty;
    }
}