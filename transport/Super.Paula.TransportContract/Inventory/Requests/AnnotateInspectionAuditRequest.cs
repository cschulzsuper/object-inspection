using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Inventory.Requests
{
    public class AnnotateInspectionAuditRequest
    {
        [StringLength(4000)]
        public string Annotation { get; set; } = string.Empty;
    }
}