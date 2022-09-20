using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public class EventWorker : IWorker
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
                    var success = await eventStorage.SetEventAsInProgressAsync(@event.Event.Id);
                    if (success)
                    {
                        await eventBus.PublishAsync(@event.Event, @event.User);
                        await eventStorage.SetEventCompletionAsync(@event.Event.Id);
                    }
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