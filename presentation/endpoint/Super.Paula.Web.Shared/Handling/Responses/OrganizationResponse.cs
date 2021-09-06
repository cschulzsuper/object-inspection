namespace Super.Paula.Web.Shared.Handling.Responses
{
    public class OrganizationResponse
    {
        public string ChiefInspector { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public bool Activated { get; set; } = false;
    }
}