namespace Super.Paula.Environment
{
    public class AppState
    {
        public string CurrentOrganization { get; set; } = string.Empty;
        public string CurrentInspector { get; set; } = string.Empty;
        public bool IgnoreCurrentOrganization { get; set; } = false;
    }
}
