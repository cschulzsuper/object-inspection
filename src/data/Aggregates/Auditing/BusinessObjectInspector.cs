namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspector
{
    public string ETag { get; set; } = string.Empty;

    public string Inspector { get; set; } = string.Empty;

    public string BusinessObject { get; set; } = string.Empty;
    public string BusinessObjectDisplayName { get; set; } = string.Empty;
}