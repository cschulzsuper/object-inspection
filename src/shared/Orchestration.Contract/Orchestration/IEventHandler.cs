using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public interface IEventHandler<in TEvent> : IEventHandler
    where TEvent : EventBase
{
    Task HandleAsync(EventHandlerContext context, TEvent @event);
}

public interface IEventHandler
{
}