using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class AnnotateInspectionAuditRequest
    {
        [StringLength(4000)]
        public string Annotation { get; set; } = string.Empty;
    }
}