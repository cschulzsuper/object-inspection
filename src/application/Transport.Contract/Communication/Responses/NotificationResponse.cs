namespace Super.Paula.Application.Communication.Responses
{
    public class NotificationResponse
    {
        public string ETag { get; set; } = string.Empty;
        public int Date { get; set; }
        public int Time { get; set; }
        public string Inspector { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}