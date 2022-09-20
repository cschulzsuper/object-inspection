using System;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record WorkerRegistration(Type WorkerType, string WorkerName)
{
    public int IterationDelay { get; set; }
}