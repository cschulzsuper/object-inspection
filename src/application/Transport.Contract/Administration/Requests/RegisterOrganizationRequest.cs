using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Requests;

public class RegisterOrganizationRequest
{
    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string UniqueName { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string DisplayName { get; set; } = string.Empty;
}