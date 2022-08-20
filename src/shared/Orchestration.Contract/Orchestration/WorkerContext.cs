using System;

namespace Super.Paula.Shared.Orchestration;

public record WorkerContext(
    IServiceProvider Services,
    int IterationDelay);