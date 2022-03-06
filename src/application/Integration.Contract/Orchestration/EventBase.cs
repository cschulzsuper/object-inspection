﻿using System;
using System.Text.Json.Serialization;

namespace Super.Paula.Application.Orchestration
{
    public record EventBase
    {
        public EventBase()
        {
            Id = Guid.NewGuid();
            (CreationDate, CreationTime) = DateTime.UtcNow.ToNumbers();
        }

        [JsonInclude]
        public Guid Id { get; private init; }

        [JsonInclude]
        public int CreationDate { get; private init; }

        [JsonInclude]
        public int CreationTime { get; private init; }
    }
}
