namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class RegisterInspectorRequest
    {
        public string Organization { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public string MailAddress { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}