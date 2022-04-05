using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IEventProcessor
    {
        void Configure(Action<EventHandlerContext> startup);
        Task ExecuteAsync<TEvent>(string subscriberName, TEvent @event, ClaimsPrincipal? user = null) where TEvent : EventBase;
    }
}