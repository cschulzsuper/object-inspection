using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public sealed class InMemoryContinuationRegistry : IContinuationRegistry
    {
        private readonly ILogger<Continuator> _logger;

        private readonly ConcurrentDictionary<string, ContinuationHandlerRegistration> _continuationHandlerRegistrations;

        public InMemoryContinuationRegistry(ILogger<Continuator> logger)
        {
            _logger = logger;
            _continuationHandlerRegistrations = new ConcurrentDictionary<string, ContinuationHandlerRegistration>();
        }

        public Type? GetContinuationsType(string ContinuationsName) 
        {         
            var exists = _continuationHandlerRegistrations.TryGetValue(ContinuationsName, out var continuationHandlerType);

            if(!exists)
            {
                _logger.LogWarning("Continuation type for a Continuations ({ContinuationsName}) was not found.", ContinuationsName);
            }

            return exists ? continuationHandlerType?.ContinuationType : null;
        }

        public Func<object, ContinuationHandlerContext, Task>? GetContinuationsHandlerCall(string ContinuationsName)
        {
            var exists = _continuationHandlerRegistrations.TryGetValue(ContinuationsName, out var continuationHandlerType);

            if (!exists)
            {
                _logger.LogWarning("Continuation handler call for a Continuations ({ContinuationsName}) was not found.", ContinuationsName);
            }

            return exists ? continuationHandlerType?.ContinuationHandlerCall : null;
        }

        public void Register<TContinuation, THandler>(string ContinuationsName)
            where TContinuation : ContinuationBase
            where THandler : IContinuationHandler<TContinuation>
        {
            var continuationType = typeof(TContinuation);
            var continuationHandlerType = typeof(THandler);

            var existingRegistrations = _continuationHandlerRegistrations
                .Where(x => x.Value.ContinuationType == continuationType);

            if (existingRegistrations.Any())
            {
                _logger.LogWarning("A continuation registration ({continuationType}) already exists ({continuationHandlerType}).",
                    continuationType, existingRegistrations.First().Value.ContinuationHandlerType);

                return;
            }

            var continuationHandler = (IContinuationHandler<TContinuation>?)Activator.CreateInstance(typeof(THandler));
            if (continuationHandler == null)
            {
                _logger.LogWarning("Could not create continuation handler for continuation ({continuationType}).", continuationType);
                return;
            }

            var continuationHandlerCall = (object continuation, ContinuationHandlerContext context)
                => continuationHandler.HandleAsync(context, (TContinuation)continuation);

            var continuationHandlerRegistration = new ContinuationHandlerRegistration(
                ContinuationsName,
                typeof(TContinuation),
                typeof(THandler),
                continuationHandler,
                continuationHandlerCall);

            _continuationHandlerRegistrations.AddOrUpdate(ContinuationsName, continuationHandlerRegistration, (_, _) => continuationHandlerRegistration);
        }

        public void Unregister(string ContinuationsName)
        {
            var removed = _continuationHandlerRegistrations.TryRemove(ContinuationsName, out _);

            if (!removed)
            {
                _logger.LogWarning("Continuation registration ({ContinuationsName}) not found.", ContinuationsName);
            }
        }
    }
}
