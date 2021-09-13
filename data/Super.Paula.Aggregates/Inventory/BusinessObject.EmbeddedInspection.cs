namespace Super.Paula.Inventory
{
    public partial class BusinessObject
    {
        public class EmbeddedInspection
        {
            public bool Activated { get; set; } = false;
            public bool ActivatedGlobally { get; set; } = false;

            public string UniqueName { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public string Text { get; set; } = string.Empty;

            public int AuditTime { get; set; }
            public int AuditDate { get; set; }
            public string AuditInspector { get; set; } = string.Empty;
            public string AuditResult { get; set; } = string.Empty;
            public string AuditAnnotation { get; set; } = string.Empty;
        }
    }
}
