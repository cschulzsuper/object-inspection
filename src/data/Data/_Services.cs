using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidlines;
using Super.Paula.Application.Inventory;
using Super.Paula.Data.Mappings;
using Super.Paula.Data.Mappings.Administration;
using Super.Paula.Data.Mappings.Auditing;
using Super.Paula.Data.Mappings.Communication;
using Super.Paula.Data.Mappings.Guidlines;
using Super.Paula.Data.Mappings.Inventory;
using Super.Paula.Environment;

namespace Super.Paula.Data
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerData(this IServiceCollection services, bool isDevelopment)
        {
            services.AddDbContext<PaulaContext>((services, options) =>
            {
                var appSeetings = services.GetRequiredService<AppSettings>();

                options.UseCosmos(
                    appSeetings.CosmosEndpoint,
                    appSeetings.CosmosKey,
                    "Paula",
                    options =>
                    {
                        if (isDevelopment)
                        {
                            options.HttpClientFactory(() =>
                            {
                                HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                                {
                                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                                };

                                return new HttpClient(httpMessageHandler);
                            });
                            options.ConnectionMode(ConnectionMode.Gateway);
                        }
                    });
                
                options.LogTo(Console.WriteLine);
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IPartitionKeyValueGenerator<BusinessObject>, BusinessObjectPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<BusinessObjectInspectionAudit>, BusinessObjectInspectionAuditPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Inspection>, InspectionPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Inspector>, InspectorPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Notification>, NotificationPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Organization>, OrganizationPartitionKeyValueGenerator>();

            return services;
        }
    }
}
