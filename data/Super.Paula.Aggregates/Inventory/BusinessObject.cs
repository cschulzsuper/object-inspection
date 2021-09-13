namespace Super.Paula.Inventory
{
    public partial class BusinessObject
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;

        public ISet<EmbeddedInspection> Inspections { get; set; } = new HashSet<EmbeddedInspection>();
    }
}
