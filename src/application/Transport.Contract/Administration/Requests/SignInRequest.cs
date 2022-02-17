using System.ComponentModel.DataAnnotations;
using Super.Paula.Validation;

namespace Super.Paula.Application.Administration.Requests
{
    public class SignInRequest
    {
        [Required]
        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string Identity { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string Secret { get; set; } = string.Empty;
    }
}