namespace ChristianSchulz.ObjectInspection.Shared.Environment;

public class BuildInfo
{
    public string Runtime { get; set; } = string.Empty;

    public string Build { get; set; } = string.Empty;

    public string Hash { get; set; } = string.Empty;

    public string ShortHash { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;
}