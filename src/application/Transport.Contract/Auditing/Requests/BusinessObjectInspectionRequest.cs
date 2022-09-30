using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Requests;

public class BusinessObjectInspectionRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string BusinessObjectDisplayName { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string Inspection { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string InspectionDisplayName { get; set; } = string.Empty;

    [StringLength(4000)]
    public string InspectionText { get; set; } = string.Empty;

    public bool Activated { get; set; } = false;
}