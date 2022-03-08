using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Auth.Requests
{
    public class SignInIdentityRequest
    {
        [Required]
        [StringLength(140)]
        public string Secret { get; set; } = string.Empty;
    }
}