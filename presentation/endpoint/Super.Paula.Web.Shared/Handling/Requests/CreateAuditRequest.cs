namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class CreateAuditRequest
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool Activated { get; set; }
    }
}