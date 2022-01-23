using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Inventory;
using Super.Paula.Environment;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Steps
{
    public class BusinessObjectInspectionAuditScheduleCalculation : IStep
    {
        private readonly IOrganizations _organizations;
        private readonly IServiceProvider _serviceProvider;

        public BusinessObjectInspectionAuditScheduleCalculation(
            IOrganizations organizations,
            IServiceProvider serviceProvider)
        {
            _serviceProvider=serviceProvider;
            _organizations = organizations;
        }

        public async Task ExecuteAsync()
        {
            var organizations = _organizations.GetAllUniqueNames();

            foreach(var organiztaion in organizations)
            {
                using var serviceScope = _serviceProvider.CreateScope();

                var appState = serviceScope.ServiceProvider
                    .GetRequiredService<AppState>();

                appState.CurrentOrganization = organiztaion;

                var businessObjects = serviceScope.ServiceProvider
                    .GetRequiredService<IBusinessObjects>()
                    .GetAllUniqueNames();

                foreach (var businessObject in businessObjects)
                {
                    var businessObjectHandler = serviceScope.ServiceProvider
                        .GetRequiredService<IBusinessObjectHandler>();

                    await businessObjectHandler.TimeInspectionAuditAsync(businessObject);
                }
            }
        }
    }
}
