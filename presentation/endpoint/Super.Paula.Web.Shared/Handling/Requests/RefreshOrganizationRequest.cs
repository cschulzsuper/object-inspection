namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class RefreshOrganizationRequest
    {
        public string DisplayName { get; set; } = string.Empty;
        public bool Activated { get; set; }
    }
}