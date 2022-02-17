namespace Super.Paula.Application.Administration.Responses
{
    public class IdentityInspectorResponse
    {
        public string Identity { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public bool Activated { get; set; }
    }
}