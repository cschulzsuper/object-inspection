using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class ContinuationStorageWorker : IWorker
    {
        public async Task ExecuteAsync(WorkerContext context, CancellationToken cancellationToken)
        {
            var continuationStorage = context.Services.GetRequiredService<IContinuationStorage>();
            var continuator = context.Services.GetRequiredService<IContinuator>();

            while (!cancellationToken.IsCancellationRequested)
            {
                await foreach (var continuation in continuationStorage.GetPendingContinuationsAsync())
                {
                    try
                    {
                        await continuationStorage.SetContinuationAsInProgressAsync(continuation.Continuation.Id);
                        await continuator.ExecuteAsync(continuation.Continuation, continuation.User);
                        await continuationStorage.SetContinuationCompletionAsync(continuation.Continuation.Id);
                    }
                    catch (Exception exception)
                    {
                        await continuationStorage.SetContinuationFailureAsync(continuation.Continuation.Id, exception);
                    }
                }

                await Task.Delay(context.IterationDelay, cancellationToken);
            }
        }
    }
}
