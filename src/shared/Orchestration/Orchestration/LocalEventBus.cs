using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public sealed class LocalEventBus : IEventBus
{
    private record AllowedSubscription(Type EventType, string[] AllowedSubscribers);

    private readonly IList<EventSubscription> _eventSubscriptions;
    private readonly IList<AllowedSubscription> _allowedSubscriptions;

    private readonly IServiceProvider _services;
    private readonly ILogger<LocalEventBus> _logger;

    private readonly ClaimsPrincipal _defaultUser;

    private Action<IServiceProvider>? _startup = null;

    public LocalEventBus(
        IServiceProvider services,
        ILogger<LocalEventBus> logger)
    {
        _services = services;
        _logger = logger;

        _eventSubscriptions = new List<EventSubscription>();
        _allowedSubscriptions = new List<AllowedSubscription>();

        _defaultUser =
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    Enumerable.Empty<Claim>()));
    }

    public void Configure(Action<IServiceProvider> startup)
    {
        _startup = startup;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, ClaimsPrincipal? user = null)
        where TEvent : EventBase
    {
        var eventType = @event.GetType();

        var potentialSubscriptions = _eventSubscriptions
            .Where(x => x.EventType == eventType)
            .ToList();

        foreach (var subscription in potentialSubscriptions)
        {
            if (subscription.EventType == eventType &&
                subscription.EventHandler != null)
            {
                using var scope = _services.CreateScope();

                scope.ServiceProvider.ConfigureUser(user ?? _defaultUser);

                var eventProcessingStorage = scope.ServiceProvider.GetRequiredService<IEventProcessingStorage>();

                _startup?.Invoke(scope.ServiceProvider);

                await eventProcessingStorage.AddAsync(
                    subscription.SubscriberName,
                    @event,
                    user ?? _defaultUser);
            }
        }
    }

    public void Subscribe<TEvent, THandler>(string subscriberName)
        where TEvent : EventBase
        where THandler : IEventHandler<TEvent>
    {
        var eventType = typeof(TEvent);
        var eventHandlerType = typeof(THandler);

        EnsureAllowedSubscriptionInitialized(eventType);

        var subscriptionAllowed = _allowedSubscriptions
            .Any(x =>
                x.EventType == eventType &&
                x.AllowedSubscribers.Contains(subscriberName));

        if (!subscriptionAllowed)
        {
            _logger.LogWarning("Subscription ({eventType},{eventHandlerType}) for subscriber ({subscriber}) is not allowed.",
                eventType, eventHandlerType, subscriberName);

            return;
        }

        var subscriptionExists = _eventSubscriptions.Any(x =>
            x.SubscriberName == subscriberName &&
            x.EventType == eventType);

        if (subscriptionExists)
        {
            _logger.LogWarning("Subscription ({eventType}) for subscriber ({subscriber}) already exists.",
                eventType, subscriberName);

            return;
        }

        var eventHandler = (IEventHandler<TEvent>?)Activator.CreateInstance(typeof(THandler));
        if (eventHandler == null)
        {
            _logger.LogWarning("Could not create event handler for event ({eventType}).", eventType);
            return;
        }

        var eventHandlerCall = (object @event, EventHandlerContext context)
            => eventHandler.HandleAsync(context, (TEvent)@event);

        var eventSubscription = new EventSubscription(
                subscriberName,
                typeof(TEvent),
                typeof(THandler),
                eventHandler,
                eventHandlerCall);

        _eventSubscriptions.Add(eventSubscription);
    }

    public void Unsubscribe<TEvent>(string subscriberName)
        where TEvent : EventBase
    {
        var eventType = typeof(TEvent);

        var subscription = _eventSubscriptions
            .SingleOrDefault(x =>
                x.SubscriberName == subscriberName &&
                x.EventType == eventType);

        if (subscription == null)
        {
            _logger.LogWarning("Subscription ({subscriberName},{eventType}) not found.",
                subscriberName, eventType);

            return;
        }

        _eventSubscriptions.Remove(subscription);
    }

    private void EnsureAllowedSubscriptionInitialized(Type eventType)
    {
        var allowedSubscriptionsAlreadyInitialized = _allowedSubscriptions
            .Any(x => x.EventType == eventType);

        if (allowedSubscriptionsAlreadyInitialized)
        {
            return;
        }

        var allowedSubscribers = eventType
            .GetCustomAttributes<AllowedSubscriberAttribute>()
            .Select(x => x.Name)
            .ToArray();

        _allowedSubscriptions.Add(
            new AllowedSubscription(eventType, allowedSubscribers));
    }

    public Func<object, EventHandlerContext, Task>? GetEventHandlerCall(string subscriberName, Type eventType)
    {
        var subscription = _eventSubscriptions
            .SingleOrDefault(x =>
                x.SubscriberName == subscriberName &&
                x.EventType == eventType);

        if (subscription == null)
        {
            _logger.LogWarning("Event handler call for a subscriber ({subscriberName}) was not found.", subscriberName);
        }

        return subscription != null ? subscription?.EventHandlerCall : null;
    }
}