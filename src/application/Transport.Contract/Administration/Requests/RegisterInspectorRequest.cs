using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Requests
{
    public class RegisterInspectorRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string ChiefInspector { get; set; } = string.Empty;
    }
}