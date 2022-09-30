using System.ComponentModel.DataAnnotations;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Requests;

public class BusinessObjectInspectionAuditAnnotationRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [StringLength(4000)]
    public string Annotation { get; set; } = string.Empty;
}