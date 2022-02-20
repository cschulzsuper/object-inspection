using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Data.Mappings;
using Super.Paula.Data.Mappings.Administration;
using Super.Paula.Data.Mappings.Auditing;
using Super.Paula.Data.Mappings.Communication;
using Super.Paula.Data.Mappings.Guidelines;
using Super.Paula.Data.Mappings.Inventory;
using Super.Paula.Environment;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Super.Paula.Data
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerData(this IServiceCollection services, bool isDevelopment)
        {
            services.AddDbContext<PaulaAdministrationContext>((services, options) =>
            {
                var appSeetings = services.GetRequiredService<AppSettings>();

                options.ReplaceService<IModelCacheKeyFactory, PaulaContextModelCacheKeyFactory>();

                options.UseCosmos(
                    appSeetings.CosmosEndpoint,
                    appSeetings.CosmosKey,
                    appSeetings.CosmosDatabase,
                    options =>
                    {
                        if (isDevelopment)
                        {
                            options.HttpClientFactory(() =>
                            {
                                HttpMessageHandler httpMessageHandler = new HttpClientHandler
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

            services.AddDbContext<PaulaApplicationContext>((services, options) =>
            {
                var appSeetings = services.GetRequiredService<AppSettings>();

                options.ReplaceService<IModelCacheKeyFactory, PaulaContextModelCacheKeyFactory>();

                options.UseCosmos(
                    appSeetings.CosmosEndpoint,
                    appSeetings.CosmosKey,
                    appSeetings.CosmosDatabase,
                    options =>
                    {
                        if (isDevelopment)
                        {
                            options.HttpClientFactory(() =>
                            {
                                HttpMessageHandler httpMessageHandler = new HttpClientHandler
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

            services.AddScoped<PaulaContextState>();

            services.AddScoped<IRepositoryCreator, RepositoryCreator>();

            services.AddScoped(RepositoryFactory<Identity,PaulaAdministrationContext>());
            services.AddScoped(RepositoryFactory<IdentityInspector, PaulaAdministrationContext>());
            services.AddScoped(RepositoryFactory<Organization, PaulaAdministrationContext>());

            services.AddScoped(RepositoryFactory<BusinessObject, PaulaApplicationContext>());
            services.AddScoped(RepositoryFactory<BusinessObjectInspectionAudit, PaulaApplicationContext>());
            services.AddScoped(RepositoryFactory<Inspection, PaulaApplicationContext>());
            services.AddScoped(RepositoryFactory<Inspector, PaulaApplicationContext>());
            services.AddScoped(RepositoryFactory<Notification, PaulaApplicationContext>());

            services.AddScoped<IPartitionKeyValueGenerator<BusinessObject>, BusinessObjectPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<BusinessObjectInspectionAudit>, BusinessObjectInspectionAuditPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Identity>, IdentityPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Inspection>, InspectionPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Inspector>, InspectorPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<IdentityInspector>, IdentityInspectorPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Notification>, NotificationPartitionKeyValueGenerator>();
            services.AddScoped<IPartitionKeyValueGenerator<Organization>, OrganizationPartitionKeyValueGenerator>();

            return services;
        }

        private static Func<IServiceProvider, IRepository<TEntity>> RepositoryFactory<TEntity, TContext>()
            where TContext : PaulaContext
            where TEntity : class
            => services =>
                new Repository<TEntity>(
                    services.GetRequiredService<TContext>(),
                    services.GetRequiredService<PaulaContextState>(),
                    services.GetRequiredService<IPartitionKeyValueGenerator<TEntity>>());
    }
}
