namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class OrganizationRequest
    {
        public string ChiefInspector { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public bool Activated { get; set; }
    }
}