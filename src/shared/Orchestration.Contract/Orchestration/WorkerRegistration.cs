using System;

namespace Super.Paula.Shared.Orchestration;

public record WorkerRegistration(Type WorkerType, string WorkerName)
{
    public int IterationDelay { get; set; }
}