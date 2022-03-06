using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public sealed class Continuator : IContinuator, IDisposable
    {
        private record Registration(
            Type ContinuationType, 
            Type ContinuationHandlerType, 
            Func<object, IContinuationHandler, ContinuationHandlerContext, Task> ContinuationHandlerCall)
        {
            public IContinuationHandler? ContinuationHandler { get; set; }
        }

        private readonly ICollection<Registration> _registrations;
        private readonly SemaphoreSlim _registrationInitializationSemaphore;

        private readonly IServiceProvider _services;
        private readonly ILogger<Continuator> _logger;

        private readonly ClaimsPrincipal _defaultUser;

        private bool _disposed;
        private Action<ContinuationHandlerContext>? _startup = null;

        public Continuator(
            IServiceProvider services, 
            ILogger<Continuator> logger)
        {
            _services = services;
            _logger = logger;

            _registrations = new List<Registration>();
            _registrationInitializationSemaphore = new SemaphoreSlim(1, 1);

            _defaultUser =
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        Enumerable.Empty<Claim>()));
        }

        public void Configure(Action<ContinuationHandlerContext> startup)
        {
            _startup = startup;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _registrations.Clear();

            _disposed = true;
            GC.SuppressFinalize(this);
        }


        public async Task ExecuteAsync<TContinuation>(TContinuation continuation, ClaimsPrincipal? user = null)
            where TContinuation : ContinuationBase
        { 
            var continuationType = continuation.GetType();

            await EnsureContinuatorInitializedAsync(continuationType);

            var registration = _registrations
                .Single(x => x.ContinuationType == continuationType);

            if (registration.ContinuationType == continuationType &&
                registration.ContinuationHandler != null)
            {
                using var scope = _services.CreateScope();

                scope.ServiceProvider.ConfigureUser(user ?? _defaultUser);

                var context = new ContinuationHandlerContext(
                    scope.ServiceProvider,
                    user ?? _defaultUser);

                _startup?.Invoke(context);

                await registration.ContinuationHandlerCall.Invoke(continuation, registration.ContinuationHandler, context);
            }
        }

         public void Register<TContinuation, THandler>()
            where TContinuation : ContinuationBase
            where THandler : IContinuationHandler<TContinuation>
        {
            var continuationType = typeof(TContinuation);
            var continuationHandlerType = typeof(THandler);

            var existingRegistration = _registrations.SingleOrDefault(x =>
                x.ContinuationType == continuationType);

            if (existingRegistration != null)
            {
                _logger.LogWarning("A continuation ({continuationType}) already exists ({continuationHandlerType}).",
                    continuationType, existingRegistration.ContinuationHandlerType);

                return;
            }

            var continuationHandlerCall = (object continuation, IContinuationHandler handler, ContinuationHandlerContext context)
                => ((THandler)handler).HandleAsync(context, (TContinuation)continuation);

            _registrations.Add(
                new Registration(typeof(TContinuation), typeof(THandler), continuationHandlerCall));
        }

        public void Unregister<TContinuation, THandler>()
            where TContinuation : ContinuationBase
            where THandler : IContinuationHandler<TContinuation>
        {
            var continuationType = typeof(TContinuation);
            var continuationHandlerType = typeof(THandler);

            var registration = _registrations.SingleOrDefault(x =>
                x.ContinuationType == continuationType &&
                x.ContinuationHandler is THandler);

            if (registration == null)
            {
                _logger.LogWarning("Continuation ({continuationType},{continuationHandlerType}) not found.",
                    continuationType, continuationHandlerType);

                return;
            }

            _registrations.Remove(registration);
        }

        private async Task EnsureContinuatorInitializedAsync(Type continuationType)
        {
            try
            {
                await _registrationInitializationSemaphore.WaitAsync();

                var registration = _registrations
                    .SingleOrDefault(x =>
                        x.ContinuationType == continuationType &&
                        x.ContinuationHandler == null);

                if(registration != null)
                {
                    var continuationHandler = (IContinuationHandler?)Activator.CreateInstance(registration.ContinuationHandlerType);

                    if (continuationHandler == null)
                    {
                        _logger.LogWarning("Could not create continuation handler for continuation ({continuationType}).", continuationType);
                        return;
                    }

                    registration.ContinuationHandler = continuationHandler;
                }
            }
            finally
            {
                _registrationInitializationSemaphore.Release();
            }
        }
    }
}
