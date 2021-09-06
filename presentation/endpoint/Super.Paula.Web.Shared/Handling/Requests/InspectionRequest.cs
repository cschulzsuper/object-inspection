namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class InspectionRequest
    {
        public bool Activated { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
    }
}