using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class EventStorageWorker : IWorker
    {
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

                await Task.Delay(context.IterationDelay, cancellationToken);
            }
        }
    }
}
