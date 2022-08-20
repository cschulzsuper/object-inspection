using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Operation.Responses;

public class ExtensionResponse
{
    public string ETag { get; set; } = string.Empty;

    public string AggregateType { get; set; } = string.Empty;

    public ISet<ExtensionFieldResponse> Fields { get; set; } = ImmutableHashSet.Create<ExtensionFieldResponse>();
}