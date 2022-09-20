namespace ChristianSchulz.ObjectInspection.Application.Communication.Requests;

public class NotificationRequest
{
    public string ETag { get; set; } = string.Empty;
    public int Date { get; set; }
    public int Time { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
}