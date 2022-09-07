using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Auditing.Requests;

public class BusinessObjectInspectorRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string BusinessObjectDisplayName { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string Inspector { get; set; } = string.Empty;
}