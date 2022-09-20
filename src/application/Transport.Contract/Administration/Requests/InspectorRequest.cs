using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Requests;

public class InspectorRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    [UniqueName]
    public string Identity { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string UniqueName { get; set; } = string.Empty;

    public bool Activated { get; set; } = false;
}