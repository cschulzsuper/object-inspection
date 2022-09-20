using System.ComponentModel.DataAnnotations;

namespace ChristianSchulz.ObjectInspection.Application.Authentication.Requests;

public class ChangeIdentitySecretRequest
{
    [Required]
    [StringLength(140)]
    public string OldSecret { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string NewSecret { get; set; } = string.Empty;
}