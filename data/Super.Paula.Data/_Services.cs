using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Super.Paula.Administration;
using Super.Paula.Auditing;
using Super.Paula.Environment;
using Super.Paula.Guidlines;
using Super.Paula.Inventory;
using Super.Paula.Mapping.PartitionKeyValueGenerators;

namespace Super.Paula
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
