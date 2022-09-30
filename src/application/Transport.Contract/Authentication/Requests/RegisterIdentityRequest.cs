using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Authentication.Requests;

public class RegisterIdentityRequest
{
    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string UniqueName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(140)]
    public string MailAddress { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string Secret { get; set; } = string.Empty;
}