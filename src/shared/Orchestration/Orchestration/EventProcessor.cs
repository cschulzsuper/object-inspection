using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public sealed class EventProcessor : IEventProcessor
{
    private readonly ILogger<EventProcessor> _logger;
    private readonly IServiceProvider _services;
    private readonly IEventBus _eventBus;

    private readonly ClaimsPrincipal _defaultUser;

    private Action<EventHandlerContext>? _startup = null;

    public EventProcessor(
        ILogger<EventProcessor> logger,
        IServiceProvider services,
        IEventBus eventBus)
    {
        _logger = logger;
        _services = services;
        _eventBus = eventBus;

        _defaultUser =
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    Enumerable.Empty<Claim>()));
    }

    public void Configure(Action<EventHandlerContext> startup)
    {
        _startup = startup;
    }

    public async Task ExecuteAsync<TEvent>(string subscriberName, TEvent @event, ClaimsPrincipal? user = null)
        where TEvent : EventBase
    {
        var eventHandlerCall = _eventBus.GetEventHandlerCall(subscriberName, @event.GetType());

        if (eventHandlerCall == null)
        {
            _logger.LogWarning("Could not get event handler call for a event ({subscriberName},{@event},{user}).", subscriberName, @event, user);
            return;
        }

        using var scope = _services.CreateScope();

        scope.ServiceProvider.ConfigureUser(user ?? _defaultUser);

        var context = new EventHandlerContext(
            scope.ServiceProvider,
            user ?? _defaultUser);

        _startup?.Invoke(context);

        await eventHandlerCall.Invoke(@event, context);
    }
}