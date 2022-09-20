namespace ChristianSchulz.ObjectInspection.Application.Auditing.Responses;

public class BusinessObjectInspectorResponse
{
    public string ETag { get; set; } = string.Empty;

    public string Inspector { get; set; } = string.Empty;
    public string BusinessObject { get; set; } = string.Empty;
    public string BusinessObjectDisplayName { get; set; } = string.Empty;
}