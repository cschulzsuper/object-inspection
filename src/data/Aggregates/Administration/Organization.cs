namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class Organization
{
    public long Id { get; set; }

    public string ETag { get; set; } = string.Empty;

    public string UniqueName { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public bool Activated { get; set; }

    public string ChiefInspector { get; set; } = string.Empty;
}