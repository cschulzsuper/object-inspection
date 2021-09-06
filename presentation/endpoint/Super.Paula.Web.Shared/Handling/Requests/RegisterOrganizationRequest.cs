namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class RegisterOrganizationRequest
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public string ChiefInspectorMail { get; set; } = string.Empty;
        public string ChiefInspectorName { get; set; } = string.Empty;
        public string ChiefInspectorSecret { get; set; } = string.Empty;
    }
}