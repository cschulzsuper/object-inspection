namespace Super.Paula.Application.Guidelines.Responses;

public class InspectionResponse
{
    public string ETag { get; set; } = string.Empty;
    public bool Activated { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
}