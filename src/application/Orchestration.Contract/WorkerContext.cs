using System;

namespace Super.Paula.Application.Orchestration
{
    public record WorkerContext(
        IServiceProvider Services,
        int IterationDelay);
}