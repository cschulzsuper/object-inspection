namespace Super.Paula.Application.Auth;

public class Identity
{
    public string ETag { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
    public string MailAddress { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}