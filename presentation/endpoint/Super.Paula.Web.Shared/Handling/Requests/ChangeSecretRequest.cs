using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class ChangeSecretRequest
    {
        [StringLength(140)]
        public string OldSecret { get; set; } = string.Empty;

        [StringLength(140)]
        public string NewSecret { get; set; } = string.Empty;
    }
}