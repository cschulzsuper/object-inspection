using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Data;
using Super.Paula.Shared;
using Super.Paula.Shared.Orchestration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory;

public class BusinessObjectWorker : IWorker
{
    public async Task ExecuteAsync(WorkerContext context, CancellationToken cancellationToken)
    {
        var organizations = context.Services.GetRequiredService<IOrganizationQueries>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var organizationUniqueNames = organizations.GetAllUniqueNames();

            foreach (var organization in organizationUniqueNames)
            {
                using var serviceScope = context.Services.CreateScope();
                SetupScope(organization, serviceScope);

                var businessObjects = serviceScope.ServiceProvider
                    .GetRequiredService<IBusinessObjectQueries>()
                    .GetAllUniqueNames();

                foreach (var businessObject in businessObjects)
                {
                    var businessObjectInspectionHandler = serviceScope.ServiceProvider
                        .GetRequiredService<IBusinessObjectInspectionRequestHandler>();

                    await businessObjectInspectionHandler.RecalculateInspectionAuditAppointmentsAsync(businessObject);
                }
            }

            await Task.Delay(context.IterationDelay, cancellationToken);
        }
    }

    private static void SetupScope(string organization, IServiceScope scope)
    {
        scope.ServiceProvider.ConfigureUser(
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim("Organization", organization)
                    })));

        scope.ServiceProvider.ConfigureData();
    }
}