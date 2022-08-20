using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public interface IEventBus
{
    void Configure(Action<IServiceProvider> startup);

    Task PublishAsync<TEvent>(TEvent @event, ClaimsPrincipal? user = null)
        where TEvent : EventBase;

    Func<object, EventHandlerContext, Task>? GetEventHandlerCall(string subscriberName, Type eventType);

    void Subscribe<TEvent, THandler>(string subscriberName)
        where TEvent : EventBase
        where THandler : IEventHandler<TEvent>;

    void Unsubscribe<TEvent>(string subscriberName)
        where TEvent : EventBase;
}