using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Data.Mapping.PartitionKeyValueGenerators;
using Super.Paula.Environment;
using System.Diagnostics.CodeAnalysis;
using Super.Paula.Aggregates.Administration;
using Super.Paula.Aggregates.Auditing;
using Super.Paula.Aggregates.Guidlines;
using Super.Paula.Aggregates.Inventory;

namespace Super.Paula.Data
{
    [SuppressMessage("Style", "IDE1006")]
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
            services.AddScoped<IPartitionKeyValueGenerator<Inspector>, InspectorPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Organization>, OrganizationPartitionKeyValueGenerator>();

            return services;
        }
    }
}
