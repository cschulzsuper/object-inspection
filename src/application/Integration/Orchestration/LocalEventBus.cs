using Microsoft.Extensions.DependencyInjection;
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
        private record Subscription(Type EventType, Type EventHandlerType)
        {
            public IEventHandler? EventHandler { get; set; }

            public Attribute[]? Annotations { get; set; }
        }

        private readonly ICollection<Subscription> _subscriptions;
        private readonly SemaphoreSlim _subscriptionInitializatrionSemaphore;

        private record AllowedSubscription(Type EventType, string[] AllowedSubscribers);

        private readonly ICollection<AllowedSubscription> _allowedSubscriptions;
        private readonly SemaphoreSlim _allowedSubscriptionInitializatrionSemaphore;

        private readonly IServiceProvider _services;
        private readonly ClaimsPrincipal _defaultUser;

        private bool _disposed;
        private Action<EventHandlerContext>? _startup = null;

        public LocalEventBus(IServiceProvider services)
        {
            _services = services;

            _subscriptions = new List<Subscription>();
            _subscriptionInitializatrionSemaphore = new SemaphoreSlim(1, 1);

            _allowedSubscriptions = new List<AllowedSubscription>();
            _allowedSubscriptionInitializatrionSemaphore = new SemaphoreSlim(1, 1);

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
                .Where(x => x.EventType == eventType);


            foreach (var subscription in potentialSubscriptions)
            {
                if (subscription.EventHandler is IEventHandler<TEvent> eventHandler)
                {
                    using var scope = _services.CreateScope();

                    scope.ServiceProvider.ConfigureUser(user ?? _defaultUser);

                    var context = new EventHandlerContext(
                        scope.ServiceProvider,
                        user ?? _defaultUser,
                        subscription.Annotations ?? Array.Empty<Attribute>());

                    _startup?.Invoke(context);

                    await eventHandler.HandleAsync(context, @event);
                }
            }
        }

        public void Subscribe<TEvent, THandler>(string subscriberName)
            where TEvent : EventBase
            where THandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);

            EnsureAllowedSubscriptionInitialized(eventType);

            var subscriptionAllowed = _allowedSubscriptions
                .Any(x =>
                    x.EventType == eventType &&
                    x.AllowedSubscribers.Contains(subscriberName));

            if (!subscriptionAllowed)
            {
                throw new LocalEventBusException($"Subscription for subscriber '{subscriberName}' is not allowed.");
            }

            var eventHandlerType = typeof(THandler);

            var subscriptionExists = _subscriptions.Any(x =>
                x.EventType == eventType &&
                x.EventHandler is THandler);

            if (subscriptionExists)
            {
                throw new LocalEventBusException($"Subscription already present.");
            }

            _subscriptions.Add(
                new Subscription(typeof(TEvent), typeof(THandler)));
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
                throw new LocalEventBusException($"Subscription not found.");
            }

            _subscriptions.Remove(subscription);
        }

        private async Task EnsureSubscriptionInitializedAsync(Type eventType)
        {
            try
            {
                await _subscriptionInitializatrionSemaphore.WaitAsync();

                var potentialSubscriptions = _subscriptions
                    .Where(x =>
                        x.EventType == eventType &&
                        x.EventHandler == null);

                foreach (var subscription in potentialSubscriptions)
                {
                    subscription.EventHandler = (IEventHandler)_services
                        .GetRequiredService(subscription.EventHandlerType);

                    subscription.Annotations = subscription.EventHandler
                        .GetType()
                        .GetCustomAttributes()
                        .ToArray();
                }

            }
            finally
            {
                _subscriptionInitializatrionSemaphore.Release();
            }
        }

        private void EnsureAllowedSubscriptionInitialized(Type eventType)
        {
            try
            {
                _allowedSubscriptionInitializatrionSemaphore.Wait();

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
                _allowedSubscriptionInitializatrionSemaphore.Release();
            }


        }
    }
}
