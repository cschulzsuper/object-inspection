namespace Super.Paula.Web.Shared.Handling.Responses
{
    public class ChangeInspectionAuditRatingResponse
    {
        public int AuditDate { get; set; }
        public string Inspection { get; set; } = string.Empty;
        public int AuditTime { get; set; }
        public string Rating { get; set; } = string.Empty;
        public string BusinessObject { get; set; } = string.Empty;
    }
}