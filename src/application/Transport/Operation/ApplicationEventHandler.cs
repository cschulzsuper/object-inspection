using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Orchestration;
using Super.Paula.Authorization;
using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
    public class ApplicationEventHandler : IApplicationEventHandler
    {
        public async Task HandleAsync(EventHandlerContext context, OrganizationCreationEvent @event)
        {
            var organizationValid = context.User.GetOrganization().Equals(@event.UniqueName);

            if (!organizationValid)
            {
                return;
            }

            var applicationManager = context.Services.GetRequiredService<IApplicationManager>();

            await applicationManager.InitializeAsync();
        }

        public async Task HandleAsync(EventHandlerContext context, OrganizationDeletionEvent @event)
        {
            var organizationValid = context.User.GetOrganization().Equals(@event.UniqueName);

            if (!organizationValid)
            {
                return;
            }

            var applicationManager = context.Services.GetRequiredService<IApplicationManager>();

            await applicationManager.PrugeAsync();
        }
    }
}
