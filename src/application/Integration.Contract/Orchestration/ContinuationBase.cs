using System;
using System.Text.Json.Serialization;

namespace Super.Paula.Application.Orchestration
{
    public record ContinuationBase
    {
        public ContinuationBase(Guid predecessor)
        {
            Id = Guid.NewGuid();
            (CreationDate, CreationTime) = DateTime.UtcNow.ToNumbers();
            Predecessors = predecessor;
        }

        [JsonInclude]
        public Guid Id { get; private init; }

        [JsonInclude]
        public Guid Predecessors { get; set; }

        [JsonInclude]
        public int CreationDate { get; private init; }

        [JsonInclude]
        public int CreationTime { get; private init; }
    }
}
