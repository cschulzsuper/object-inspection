using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public sealed class LocalEventBus : IEventBus, IDisposable
    {
        private record Subscription(
            Type EventType, 
            Type EventHandlerType, 
            string Subscriber, Func<EventBase, IEventHandler, EventHandlerContext, Task> EventHandlerCall)
        {
            public IEventHandler? EventHandler { get; set; }

            public Attribute[]? Annotations { get; set; }
        }

        private readonly ICollection<Subscription> _subscriptions;
        private readonly SemaphoreSlim _subscriptionInitializationSemaphore;

        private record AllowedSubscription(Type EventType, string[] AllowedSubscribers);

        private readonly ICollection<AllowedSubscription> _allowedSubscriptions;
        private readonly SemaphoreSlim _allowedSubscriptionInitializationSemaphore;

        private readonly IServiceProvider _services;
        private readonly ILogger<LocalEventBus> _logger;

        private readonly ClaimsPrincipal _defaultUser;

        private bool _disposed;
        private Action<EventHandlerContext>? _startup = null;

        public LocalEventBus(
            IServiceProvider services, 
            ILogger<LocalEventBus> logger)
        {
            _services = services;
            _logger = logger;

            _subscriptions = new List<Subscription>();
            _subscriptionInitializationSemaphore = new SemaphoreSlim(1, 1);

            _allowedSubscriptions = new List<AllowedSubscription>();
            _allowedSubscriptionInitializationSemaphore = new SemaphoreSlim(1, 1);

            _defaultUser =
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        Enumerable.Empty<Claim>()));
        }

        public void Configure(Action<EventHandlerContext> startup)
        {
            _startup = startup;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _subscriptions.Clear();

            _disposed = true;
            GC.SuppressFinalize(this);
        }


        public async Task PublishAsync<TEvent>(TEvent @event, ClaimsPrincipal? user = null)
            where TEvent : EventBase
        {
            var eventType = @event.GetType();

            await EnsureSubscriptionInitializedAsync(eventType);

            var potentialSubscriptions = _subscriptions
                .Where(x => x.EventType == eventType)
                .ToList();

            LogWarningForMissingSubscribers(potentialSubscriptions, @event, user, eventType);

            foreach (var subscription in potentialSubscriptions)
            {
                if (subscription.EventType == eventType &&
                    subscription.EventHandler != null)
                {
                    using var scope = _services.CreateScope();

                    scope.ServiceProvider.ConfigureUser(user ?? _defaultUser);

                    var context = new EventHandlerContext(
                        scope.ServiceProvider,
                        user ?? _defaultUser,
                        subscription.Annotations ?? Array.Empty<Attribute>());

                    _startup?.Invoke(context);

                    await subscription.EventHandlerCall.Invoke(@event, subscription.EventHandler, context);
                }
            }
        }

        private void LogWarningForMissingSubscribers(ICollection<Subscription> potentialSubscriptions, EventBase @event, ClaimsPrincipal? user, Type eventType)
        {
            if (!_logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            var potentialSubscribers = potentialSubscriptions
                .Select(x => x.Subscriber);

            var allowedSubscribers = _allowedSubscriptions
                .Where(x => x.EventType == eventType)
                .SelectMany(x => x.AllowedSubscribers);

            var missingSubscribers = allowedSubscribers
                .Where(x => !potentialSubscribers.Contains(x))
                .ToList();

            if (missingSubscribers.Any())
            {
                _logger.LogWarning("Incomplete subscription ({event}, {user}, {missingSubscribers})", @event, user, missingSubscribers);
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

            var subscriptionExists = _subscriptions.Any(x =>
                x.EventType == eventType &&
                x.EventHandler is THandler);

            if (subscriptionExists)
            {
                _logger.LogWarning("Subscription ({eventType},{eventHandlerType}) for subscriber ({subscriber}) already exists.",
                    eventType, eventHandlerType, subscriberName);

                return;
            }

            var eventHandlerCall = (EventBase @event, IEventHandler handler, EventHandlerContext context) 
                => ((THandler)handler).HandleAsync(context,(TEvent)@event);

            _subscriptions.Add(
                new Subscription(typeof(TEvent), typeof(THandler), subscriberName, eventHandlerCall));
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : EventBase
            where THandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var eventHandlerType = typeof(THandler);

            var subscription = _subscriptions.SingleOrDefault(x =>
                x.EventType == eventType &&
                x.EventHandler is THandler);

            if (subscription == null)
            {
                _logger.LogWarning("Subscription ({eventType},{eventHandlerType}) not found.",
                    eventType, eventHandlerType);

                return;
            }

            _subscriptions.Remove(subscription);
        }

        private async Task EnsureSubscriptionInitializedAsync(Type eventType)
        {
            try
            {
                await _subscriptionInitializationSemaphore.WaitAsync();

                var potentialSubscriptions = _subscriptions
                    .Where(x =>
                        x.EventType == eventType &&
                        x.EventHandler == null);

                foreach (var subscription in potentialSubscriptions)
                {
                    var eventHandler = (IEventHandler?)Activator.CreateInstance(subscription.EventHandlerType);

                    if (eventHandler == null)
                    {
                        _logger.LogWarning("Could not create event handler for event ({eventType}).", eventType);
                        continue;
                    }

                    subscription.EventHandler = eventHandler;
                    subscription.Annotations = subscription.EventHandler
                        .GetType()
                        .GetCustomAttributes()
                        .ToArray();
                }

            }
            finally
            {
                _subscriptionInitializationSemaphore.Release();
            }
        }

        private void EnsureAllowedSubscriptionInitialized(Type eventType)
        {
            try
            {
                _allowedSubscriptionInitializationSemaphore.Wait();

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
            finally
            {
                _allowedSubscriptionInitializationSemaphore.Release();
            }


        }
    }
}
