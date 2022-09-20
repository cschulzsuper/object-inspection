using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public sealed class Continuator : IContinuator
{
    private readonly ILogger<Continuator> _logger;
    private readonly IServiceProvider _services;
    private readonly IContinuationRegistry _continuationRegistry;

    private readonly ClaimsPrincipal _defaultUser;

    private Action<ContinuationHandlerContext>? _startup = null;

    public Continuator(
        ILogger<Continuator> logger,
        IServiceProvider services,
        IContinuationRegistry continuationRegistry)
    {
        _logger = logger;
        _services = services;
        _continuationRegistry = continuationRegistry;

        _defaultUser =
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    Enumerable.Empty<Claim>()));
    }

    public void Configure(Action<ContinuationHandlerContext> startup)
    {
        _startup = startup;
    }

    public async Task ExecuteAsync<TContinuation>(TContinuation continuation, ClaimsPrincipal? user = null)
        where TContinuation : ContinuationBase
    {
        var continuationName = TypeNameConverter.ToKebabCase(continuation.GetType());
        var continuationCall = _continuationRegistry.GetContinuationHandlerCall(continuationName);

        if (continuationCall == null)
        {
            _logger.LogWarning("Could not get continuation handler call for a continuation ({ContinuationName},{user}).", continuation, user);
            return;
        }

        using var scope = _services.CreateScope();

        scope.ServiceProvider.ConfigureUser(user ?? _defaultUser);

        var context = new ContinuationHandlerContext(
            scope.ServiceProvider,
            user ?? _defaultUser);

        _startup?.Invoke(context);

        await continuationCall.Invoke(continuation, context);
    }
}