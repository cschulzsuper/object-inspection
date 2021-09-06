namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class AssignInspectionRequest
    {
        public bool Activated { get; set; } = false;
        public string UniqueName { get; set; } = string.Empty;
    }
}