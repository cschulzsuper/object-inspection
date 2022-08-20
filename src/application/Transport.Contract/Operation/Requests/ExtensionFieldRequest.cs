using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Operation.Requests;

public class ExtensionFieldRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string UniqueName { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [LowerCase]
    [StringLength(140)]
    [ExtensionFieldDataType]
    public string DataType { get; set; } = string.Empty;

    [Required]
    [CamelCase]
    [StringLength(140)]
    [ExtensionFieldDataName]
    public string DataName { get; set; } = string.Empty;
}