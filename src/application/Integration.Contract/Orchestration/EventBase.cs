using System;
using System.Text.Json.Serialization;

namespace Super.Paula.Application.Orchestration
{
    public record EventBase
    {
        public EventBase()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonInclude]
        public Guid Id { get; private init; }

        [JsonInclude]
        public DateTime CreationDate { get; private init; }
    }
}
