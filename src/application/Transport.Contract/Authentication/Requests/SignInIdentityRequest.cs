using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Authentication.Requests;

public class SignInIdentityRequest
{
    [Required]
    [StringLength(140)]
    public string Secret { get; set; } = string.Empty;
}