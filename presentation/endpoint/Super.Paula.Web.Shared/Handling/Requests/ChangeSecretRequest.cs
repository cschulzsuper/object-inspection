using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class ChangeSecretRequest
    {
        [Required]
        [StringLength(140)]
        public string OldSecret { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        public string NewSecret { get; set; } = string.Empty;
    }
}