using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IEventBus
    {
        void Configure(Action<EventHandlerContext> startup);

        Task PublishAsync<TEvent>(TEvent @event, ClaimsPrincipal? user = null)
            where TEvent : EventBase;

        void Subscribe<TEvent, THandler>(string subscriberName)
            where TEvent : EventBase
            where THandler : IEventHandler<TEvent>;

        void Unsubscribe<TEvent, THandler>()
            where TEvent : EventBase
            where THandler : IEventHandler<TEvent>;
    }
}
