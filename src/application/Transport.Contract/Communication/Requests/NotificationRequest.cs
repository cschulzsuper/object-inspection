namespace Super.Paula.Application.Communication.Requests
{
    public class NotificationRequest
    {
        public int Date { get; set; }
        public int Time { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
    }
}