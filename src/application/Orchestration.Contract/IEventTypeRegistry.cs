using System;

namespace Super.Paula.Application.Orchestration
{
    public interface IEventTypeRegistry
    {
        Type? GetEventType(string eventName);

        void Register<TEvent>(string eventName)
            where TEvent : EventBase;

        void Unregister(string eventName);
    }
}