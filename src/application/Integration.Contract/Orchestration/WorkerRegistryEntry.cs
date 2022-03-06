using System;

namespace Super.Paula.Application.Orchestration
{
    public record WorkerRegistryEntry(Type WorkerType)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}
