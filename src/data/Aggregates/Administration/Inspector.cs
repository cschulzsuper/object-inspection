using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class Inspector
{
    public long Id { get; set; }
    public string ETag { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
    public string Identity { get; set; } = string.Empty;
    public bool Activated { get; set; }

    public string Organization { get; set; } = string.Empty;
    public string OrganizationDisplayName { get; set; } = string.Empty;
    public bool OrganizationActivated { get; set; }

    public ISet<InspectorBusinessObject> BusinessObjects { get; set; } = new HashSet<InspectorBusinessObject>();
}