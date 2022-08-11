using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Orchestration;
using Super.Paula.Authorization;
using Super.Paula.Environment;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
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
}
