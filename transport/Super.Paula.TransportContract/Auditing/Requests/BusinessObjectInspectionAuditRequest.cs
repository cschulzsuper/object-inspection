using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Auditing.Requests
{
    public class BusinessObjectInspectionAuditRequest
    {
        [StringLength(4000)]
        public string Annotation { get; set; } = string.Empty;

        public int AuditDate { get; set; }

        public int AuditTime { get; set; }

        [Required]
        [KebabCase]
        [StringLength(140)]
        [InvalidValues("search")]
        public string Inspection { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        public string Inspector { get; set; } = string.Empty;

        [Required]
        [ValidValues("satisfying", "insufficient", "failed")]
        public string Result { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string BusinessObjectDisplayName { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string InspectionDisplayName { get; set; } = string.Empty;
    }
}