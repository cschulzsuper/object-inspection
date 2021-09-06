namespace Super.Paula.Web.Shared.Handling.Responses
{
    public class InspectorResponse
    {
        public string MailAddress { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public bool Activated { get; set; } = false;
    }
}