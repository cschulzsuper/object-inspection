using System.Collections.Generic;
using System.Collections.Immutable;

namespace ChristianSchulz.ObjectInspection.Application.Operation.Responses;

public class DistinctionTypeResponse
{
    public string ETag { get; set; } = string.Empty;

    public string UniqueName { get; set; } = string.Empty;

    public ISet<DistinctionTypeFieldResponse> Fields { get; set; } = ImmutableHashSet.Create<DistinctionTypeFieldResponse>();
}