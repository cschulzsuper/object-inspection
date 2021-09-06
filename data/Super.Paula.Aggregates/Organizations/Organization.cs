namespace Super.Paula.Aggregates.Organizations
{
    public class Organization
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool Activated { get; set; }

        public string ChiefInspector { get; set; } = string.Empty;
    }
}
