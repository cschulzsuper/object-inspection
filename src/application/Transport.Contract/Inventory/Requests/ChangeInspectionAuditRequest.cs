using System.ComponentModel.DataAnnotations;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Requests
{
    public class ChangeInspectionAuditRequest
    {
        [Required]
        [AuditResult]
        public string Result { get; set; } = string.Empty;
    }
}