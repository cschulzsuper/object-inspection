using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Administration.Events;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

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

    private static void Guard(EventHandlerContext context)
    {
        var appSettings = context.Services.GetRequiredService<AppSettings>();

        var contextUserIdentity = context.User.Claims.GetIdentity();
        var contextUserIdentityUnauthorized =
            appSettings.MaintainerIdentity != contextUserIdentity;

        if (contextUserIdentityUnauthorized)
        {
            throw new TransportException($"Unauthorized identity '{contextUserIdentity}'");
        }
    }
}