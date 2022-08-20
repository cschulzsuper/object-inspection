using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public sealed class InMemoryContinuationRegistry : IContinuationRegistry
{
    private readonly ILogger<InMemoryContinuationRegistry> _logger;

    private readonly ConcurrentDictionary<string, ContinuationRegistration> _continuationRegistrations;

    public InMemoryContinuationRegistry(ILogger<InMemoryContinuationRegistry> logger)
    {
        _logger = logger;
        _continuationRegistrations = new ConcurrentDictionary<string, ContinuationRegistration>();
    }

    public Type? GetContinuationType(string continuationName)
    {
        var exists = _continuationRegistrations.TryGetValue(continuationName, out var continuationRegistration);

        if (!exists)
        {
            _logger.LogWarning("Continuation type for a continuation ({continuationName}) was not found.", continuationName);
        }

        return exists ? continuationRegistration?.ContinuationType : null;
    }

    public Func<object, ContinuationHandlerContext, Task>? GetContinuationHandlerCall(string continuationName)
    {
        var exists = _continuationRegistrations.TryGetValue(continuationName, out var continuationRegistration);

        if (!exists)
        {
            _logger.LogWarning("Continuation handler call for a continuation ({continuationName}) was not found.", continuationName);
        }

        return exists ? continuationRegistration?.ContinuationHandlerCall : null;
    }

    public void Register<TContinuation, THandler>()
        where TContinuation : ContinuationBase
        where THandler : IContinuationHandler<TContinuation>
    {
        var continuationType = typeof(TContinuation);
        var continuationHandlerType = typeof(THandler);
        var continuationName = TypeNameConverter.ToKebabCase(continuationType);

        var continuationHandler = (IContinuationHandler<TContinuation>?)Activator.CreateInstance(typeof(THandler));
        if (continuationHandler == null)
        {
            _logger.LogWarning("Could not create continuation handler for continuation ({continuationType}).", continuationType);
            return;
        }

        var continuationHandlerCall = (object continuation, ContinuationHandlerContext context)
            => continuationHandler.HandleAsync(context, (TContinuation)continuation);

        var continuationHandlerRegistration = new ContinuationRegistration(
            typeof(TContinuation),
            typeof(THandler),
            continuationHandler,
            continuationHandlerCall);

        _continuationRegistrations.AddOrUpdate(continuationName, continuationHandlerRegistration,
            (_, exitingRegistration) =>
            {
                _logger.LogWarning("A continuation registration with the same name ({continuationName}) already exists.", continuationName);
                return exitingRegistration;
            });
    }

    public void Unregister<TContinuation>()
        where TContinuation : ContinuationBase
    {
        var continuationType = typeof(TContinuation);
        var continuationName = TypeNameConverter.ToKebabCase(continuationType);

        var removed = _continuationRegistrations.TryRemove(continuationName, out _);

        if (!removed)
        {
            _logger.LogWarning("Continuation registration ({continuationName}) not found.", continuationName);
        }
    }
}