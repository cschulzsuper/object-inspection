namespace Super.Paula.Application.Authentication.Responses;

public class IdentityResponse
{
    public string ETag { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
    public string MailAddress { get; set; } = string.Empty;
}