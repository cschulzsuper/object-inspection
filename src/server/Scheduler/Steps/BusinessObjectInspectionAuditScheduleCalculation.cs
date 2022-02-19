using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Inventory;
using Super.Paula.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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

            foreach (var organiztaion in organizations)
            {
                using var serviceScope = _serviceProvider.CreateScope();

                serviceScope.ServiceProvider.ConfigureUser(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new List<Claim>
                            {
                                new Claim("Organization", organiztaion)
                            })));

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
