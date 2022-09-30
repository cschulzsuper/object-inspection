using System;
using System.Text.Json.Serialization;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record ContinuationBase
{
    public ContinuationBase()
    {
        Id = Guid.NewGuid();
        (CreationDate, CreationTime) = DateTimeNumbers.GlobalNow;
    }

    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public int CreationDate { get; private init; }

    [JsonInclude]
    public int CreationTime { get; private init; }
}