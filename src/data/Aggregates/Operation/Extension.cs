using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class Extension
{
    public string ETag { get; set; } = string.Empty;

    public string AggregateType { get; set; } = string.Empty;

    public ISet<ExtensionField> Fields { get; set; } = new HashSet<ExtensionField>();

}