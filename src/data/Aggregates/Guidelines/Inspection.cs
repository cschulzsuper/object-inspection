namespace ChristianSchulz.ObjectInspection.Application.Guidelines;

public class Inspection
{
    public long Id { get; set; }
    public string ETag { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool Activated { get; set; } = false;
}