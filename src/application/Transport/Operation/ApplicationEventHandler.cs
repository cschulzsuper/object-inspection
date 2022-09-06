using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Shared.Environment;
using Super.Paula.Shared.Orchestration;
using System.Threading.Tasks;
using Super.Paula.Shared.Security;

namespace Super.Paula.Application.Operation;

public class ApplicationEventHandler : IApplicationEventHandler
{
    public async Task HandleAsync(EventHandlerContext context, OrganizationCreationEvent @event)
    {
        Guard(context);

        var applicationManager = context.Services.GetRequiredService<IApplicationManager>();

        await applicationManager.InitializeAsync(@event.UniqueName);
    }

    public async Task HandleAsync(EventHandlerContext context, OrganizationDeletionEvent @event)
    {
        Guard(context);

        var applicationManager = context.Services.GetRequiredService<IApplicationManager>();

        await applicationManager.PrugeAsync(@event.UniqueName);
    }

    private void Guard(EventHandlerContext context)
    {
        var appSettings = context.Services.GetRequiredService<AppSettings>();

        var contextUserIdentity = context.User.GetIdentity();
        var contextUserIdentityUnauthorized =
            appSettings.MaintainerIdentity != contextUserIdentity;

        if (contextUserIdentityUnauthorized)
        {
            throw new TransportException($"Unauthorized identity '{contextUserIdentity}'");
        }
    }
}