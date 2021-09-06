namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class StartImpersonationRequest
    {
        public string Organization { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
    }
}