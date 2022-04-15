using System;
using System.Text.Json.Serialization;

namespace Super.Paula.Application.Orchestration
{
    public record ContinuationBase
    {
        public ContinuationBase(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            (CreationDate, CreationTime) = DateTimeNumbers.GlobalNow;
        }

        [JsonInclude]
        public string Name { get; private init; }

        [JsonInclude]
        public Guid Id { get; private init; }

        [JsonInclude]
        public int CreationDate { get; private init; }

        [JsonInclude]
        public int CreationTime { get; private init; }
    }
}
