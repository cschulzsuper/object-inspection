using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Operation.Requests;

public class ExtensionRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [ExtensionAggregateType]
    public string AggregateType { get; set; } = string.Empty;
}