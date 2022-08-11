using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Operation.Requests
{
    public class ExtensionRequest
    {
        public string ETag { get; set; } = string.Empty;

        [Required]
        [KebabCase]
        [StringLength(140)]
        [ExtensionAggregateType]
        public string AggregateType { get; set; } = string.Empty;
    }
}