using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Administration.Requests
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