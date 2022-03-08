using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Auth.Requests
{
    public class ChangeIdentitySecretRequest
    {
        [Required]
        [StringLength(140)]
        public string OldSecret { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string NewSecret { get; set; } = string.Empty;
    }
}