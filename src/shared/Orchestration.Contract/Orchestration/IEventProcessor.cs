using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public interface IEventProcessor
{
    void Configure(Action<EventHandlerContext> startup);
    Task ExecuteAsync<TEvent>(string subscriberName, TEvent @event, ClaimsPrincipal? user = null) where TEvent : EventBase;
}