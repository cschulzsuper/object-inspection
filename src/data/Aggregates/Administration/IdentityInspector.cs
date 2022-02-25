namespace Super.Paula.Application.Administration
{
    public class IdentityInspector
    {
        public string ETag { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;
        public bool Activated { get; set; }
    }
}
