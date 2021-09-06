namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class ChangeSecretRequest
    {
        public string OldSecret { get; set; } = string.Empty;
        public string NewSecret { get; set; } = string.Empty;
    }
}