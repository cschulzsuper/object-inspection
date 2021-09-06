namespace Super.Paula.Environment
{
    public class AppState
    {
        public virtual bool IsDevelopment { get; set; } = false;
        public virtual string CurrentOrganization { get; set; } = string.Empty;
        public virtual string CurrentInspector { get; set; } = string.Empty;
        public virtual string CurrentBearer { get; set; } = string.Empty;
        public virtual string CurrentImpersonator { get; set; } = string.Empty;
        public bool IgnoreCurrentOrganization { get; set; } = false;
    }
}
