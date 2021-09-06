using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Aggregates.BusinessObjectInspectionAudits;
using Super.Paula.Aggregates.BusinessObjects;
using Super.Paula.Aggregates.InspectionBusinessObjects;
using Super.Paula.Aggregates.Inspections;
using Super.Paula.Aggregates.Inspectors;
using Super.Paula.Aggregates.Organizations;
using Super.Paula.Data.Mapping;
using Super.Paula.Data.Mapping.PartitionKeyValueGenerators;
using Super.Paula.Environment;

namespace Super.Paula.Data
{
    public static class _Services
    {
        public static IServiceCollection AddPaulaData(this IServiceCollection services)
        {
            services.AddDbContext<PaulaContext>((services, options) =>
            {
                var appSeetings = services.GetRequiredService<AppSettings>();

                options.UseCosmos(
                    appSeetings.CosmosEndpoint,
                    appSeetings.CosmosKey,
                    "Paula");
                
                options.LogTo(Console.WriteLine);
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IPartitionKeyValueGenerator<BusinessObject>, BusinessObjectPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<BusinessObjectInspectionAudit>, BusinessObjectInspectionAuditPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Inspection>, InspectionPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<InspectionBusinessObject>, InspectionBusinessObjectPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Inspector>, InspectorPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Organization>, OrganizationPartitionKeyValueGenerator>();

            return services;
        }
    }
}
