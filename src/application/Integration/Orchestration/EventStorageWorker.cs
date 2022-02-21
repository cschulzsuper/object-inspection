using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class EventStorageWorker : IWorker
    {

        private readonly int eventStorageIterationDelay = 1_000;  // 1 second

        public async Task ExecuteAsync(WorkerContext context, CancellationToken cancellationToken)
        {
            var eventStorage = context.Services.GetRequiredService<IEventStorage>();
            var eventBus = context.Services.GetRequiredService<IEventBus>();

            while (!cancellationToken.IsCancellationRequested)
            {
                await foreach (var @event in eventStorage.GetPendingEventsAsync())
                {
                    try
                    {
                        await eventStorage.SetEventAsInProgressAsync(@event.Event.Id);
                        await eventBus.PublishAsync(@event.Event, @event.User);
                        await eventStorage.SetEventCompletionAsync(@event.Event.Id);
                    }
                    catch (Exception exception)
                    {
                        await eventStorage.SetEventFailureAsync(@event.Event.Id, exception);
                    }
                }

                await Task.Delay(eventStorageIterationDelay, cancellationToken);
            }
        }
    }
}
