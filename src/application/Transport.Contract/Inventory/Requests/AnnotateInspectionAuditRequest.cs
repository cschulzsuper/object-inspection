using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Requests
{
    public class AnnotateInspectionAuditRequest
    {
        [StringLength(4000)]
        public string Annotation { get; set; } = string.Empty;
    }
}