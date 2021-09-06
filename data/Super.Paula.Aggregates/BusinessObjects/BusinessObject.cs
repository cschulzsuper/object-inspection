namespace Super.Paula.Aggregates.BusinessObjects
{
    public partial class BusinessObject
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;

        public ISet<BusinessObjectInspection> Inspections { get; set; } = new HashSet<BusinessObjectInspection>();
    }
}
