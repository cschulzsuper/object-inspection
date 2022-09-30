using System;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public record WorkerContext(
    IServiceProvider Services,
    int IterationDelay);