namespace ChristianSchulz.ObjectInspection.Application.Communication;

public class Notification
{
    public long Id { get; set; }
    public string ETag { get; set; } = string.Empty;
    public string Inspector { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int Date { get; set; }
    public int Time { get; set; }
}