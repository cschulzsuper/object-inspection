namespace Super.Paula.Administration
{
    public class Inspector
    {
        public string UniqueName { get; set; } = string.Empty;
        public string MailAddress { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string Proof { get; set; } = string.Empty;
        public bool Activated { get; set; }

        public string Organization { get; set; } = string.Empty;
        public string OrganizationDisplayName { get; set; } = string.Empty;
        public bool OrganizationActivated { get; set; }
    }
}
