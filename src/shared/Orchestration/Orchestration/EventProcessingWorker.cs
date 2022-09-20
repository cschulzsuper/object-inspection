using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public class EventProcessingWorker : IWorker
{
    public async Task ExecuteAsync(WorkerContext context, CancellationToken cancellationToken)
    {
        var eventProcessingStorage = context.Services.GetRequiredService<IEventProcessingStorage>();
        var eventProcessor = context.Services.GetRequiredService<IEventProcessor>();

        while (!cancellationToken.IsCancellationRequested)
        {
            await foreach (var eventProcessing in eventProcessingStorage.GetPendingEventProcessingsAsync())
            {
                try
                {
                    var success = await eventProcessingStorage.SetEventProcessingAsInProgressAsync(eventProcessing.SubscriberName, eventProcessing.Event.Id);
                    if (success)
                    {
                        await eventProcessor.ExecuteAsync(eventProcessing.SubscriberName, eventProcessing.Event, eventProcessing.User);
                        await eventProcessingStorage.SetEventProcessingCompletionAsync(eventProcessing.SubscriberName, eventProcessing.Event.Id);
                    }
                }
                catch (Exception exception)
                {
                    await eventProcessingStorage.SetEventProcessingFailureAsync(eventProcessing.SubscriberName, eventProcessing.Event.Id, exception);
                }
            }

            await Task.Delay(context.IterationDelay, cancellationToken);
        }
    }
}