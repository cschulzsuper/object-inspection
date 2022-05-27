using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace Super.Paula.Application.Orchestration
{
    public sealed class InMemoryEventTypeRegistry : IEventTypeRegistry
    {
        private readonly ILogger<InMemoryEventTypeRegistry> _logger;

        private readonly ConcurrentDictionary<string, EventRegistration> _eventRegistrations;

        public InMemoryEventTypeRegistry(ILogger<InMemoryEventTypeRegistry> logger)
        {
            _logger = logger;
            _eventRegistrations = new ConcurrentDictionary<string, EventRegistration>();
        }

        public Type? GetEventType(string eventName) 
        {         
            var exists = _eventRegistrations.TryGetValue(eventName, out var eventRegistration);

            if(!exists)
            {
                _logger.LogWarning("Event type for a event ({eventName}) was not found.", eventName);
            }

            return exists ? eventRegistration?.EventType : null;
        }

        public void Register<TEvent>()
            where TEvent : EventBase
        {
            var eventType = typeof(TEvent);
            var eventName = TypeNameConverter.ToKebabCase(eventType);

            var eventRegistration = new EventRegistration(
                typeof(TEvent));

            _eventRegistrations.AddOrUpdate(eventName, eventRegistration,
                (_, exitingRegistration) =>
                {
                    _logger.LogWarning("An event registration with the same name ({eventName}) already exists.", eventName);
                    return exitingRegistration;
                });
        }

        public void Unregister<TEvent>()
            where TEvent : EventBase
        {
            var eventType = typeof(TEvent);
            var eventName = TypeNameConverter.ToKebabCase(eventType);

            var removed = _eventRegistrations.TryRemove(eventName, out _);

            if (!removed)
            {
                _logger.LogWarning("Event registration ({eventName}) not found.", eventName);
            }
        }
    }
}
