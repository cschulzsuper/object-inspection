using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class DistinctionType
{
    public long Id { get; set; }

    public string ETag { get; set; } = string.Empty;

    public string UniqueName { get; set; } = string.Empty;

    public ISet<DistinctionTypeField> Fields { get; set; } = new HashSet<DistinctionTypeField>();

}