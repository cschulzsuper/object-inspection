using System.Collections.Generic;
using System.Collections.Immutable;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Responses;

public class InspectorResponse
{
    public string ETag { get; set; } = string.Empty;

    public string Identity { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
    public bool Activated { get; set; }

    public ISet<InspectorBusinessObjectResponse> BusinessObjects { get; set; } = ImmutableHashSet.Create<InspectorBusinessObjectResponse>();
}