namespace Super.Paula.Aggregates.Inspections
{
    public class Inspection
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool Activated { get; set; } = false;
    }
}
