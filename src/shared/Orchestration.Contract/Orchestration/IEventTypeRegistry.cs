using System;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public interface IEventTypeRegistry
{
    Type? GetEventType(string eventName);

    void Register<TEvent>()
        where TEvent : EventBase;

    void Unregister<TEvent>()
        where TEvent : EventBase;
}