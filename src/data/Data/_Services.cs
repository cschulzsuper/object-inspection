using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Operation;
using Super.Paula.Application.Orchestration;
using Super.Paula.Data.Mappings;
using Super.Paula.Data.Mappings.Administration;
using Super.Paula.Data.Mappings.Auditing;
using Super.Paula.Data.Mappings.Communication;
using Super.Paula.Data.Mappings.Guidelines;
using Super.Paula.Data.Mappings.Inventory;
using Super.Paula.Data.Mappings.Operation;
using Super.Paula.Data.Mappings.Orchestration;
using Super.Paula.Shared.Environment;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Super.Paula.Application.Authentication;
using Super.Paula.Data.Mappings.Authentication;
using Super.Paula.Shared.Security;

namespace Super.Paula.Data;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerData(this IServiceCollection services, bool isDevelopment)
    {
        services.AddDbContext<PaulaAdministrationContext>((services, options) =>
        {
            var appSeetings = services.GetRequiredService<AppSettings>();

            options.ReplaceService<IModelCacheKeyFactory, PaulaAdministrationContextModelCacheKeyFactory>();

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
                                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            };

                            return new HttpClient(httpMessageHandler);
                        });
                        options.ConnectionMode(ConnectionMode.Gateway);
                    }
                });


            if (isDevelopment)
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddDbContext<PaulaApplicationContext>((services, options) =>
        {
            var appSeetings = services.GetRequiredService<AppSettings>();

            options.ReplaceService<IModelCacheKeyFactory, PaulaApplicationContextModelCacheKeyFactory>();

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
                                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            };

                            return new HttpClient(httpMessageHandler);
                        });
                        options.ConnectionMode(ConnectionMode.Gateway);
                    }
                });

            if (isDevelopment)
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddDbContext<PaulaOperationContext>((services, options) =>
        {
            var appSettings = services.GetRequiredService<AppSettings>();

            options.ReplaceService<IModelCacheKeyFactory, PaulaOperationContextModelCacheKeyFactory>();

            options.UseCosmos(
                appSettings.CosmosEndpoint,
                appSettings.CosmosKey,
                appSettings.CosmosDatabase,
                options =>
                {
                    if (isDevelopment)
                    {
                        options.HttpClientFactory(() =>
                        {
                            HttpMessageHandler httpMessageHandler = new HttpClientHandler
                            {
                                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            };

                            return new HttpClient(httpMessageHandler);
                        });
                        options.ConnectionMode(ConnectionMode.Gateway);
                    }
                });

            if (isDevelopment)
            {
                options.EnableSensitiveDataLogging();
            }
        });


        services.AddScoped<PaulaContexts>();
        services.AddScoped<PaulaContextState>();

        services.AddScoped<ExtensionProvider>();
        services.AddScoped<ExtensionCacheKeyFactory>();
        services.AddSingleton<ExtensionCache>();

        services.AddScoped<IRepositoryCreator, RepositoryCreator>();

        services.AddScoped(RepositoryFactory<Identity, PaulaAdministrationContext>());
        services.AddScoped(RepositoryFactory<IdentityInspector, PaulaAdministrationContext>());
        services.AddScoped(RepositoryFactory<Organization, PaulaAdministrationContext>());

        services.AddScoped<IRepository<Extension>, ExtensionRepository>();

        services.AddScoped(RepositoryFactory<Continuation, PaulaAdministrationContext>());
        services.AddScoped(RepositoryFactory<Event, PaulaAdministrationContext>());
        services.AddScoped(RepositoryFactory<EventProcessing, PaulaAdministrationContext>());
        services.AddScoped(RepositoryFactory<Worker, PaulaAdministrationContext>());

        services.AddScoped(RepositoryFactory<BusinessObject, PaulaApplicationContext>());
        services.AddScoped(RepositoryFactory<BusinessObjectInspector, PaulaApplicationContext>());
        services.AddScoped(RepositoryFactory<BusinessObjectInspection, PaulaApplicationContext>());
        services.AddScoped(RepositoryFactory<BusinessObjectInspectionAuditRecord, PaulaApplicationContext>());
        services.AddScoped(RepositoryFactory<Inspection, PaulaApplicationContext>());
        services.AddScoped(RepositoryFactory<Inspector, PaulaApplicationContext>());
        services.AddScoped(RepositoryFactory<Notification, PaulaApplicationContext>());

        services.AddScoped<IPartitionKeyValueGenerator<BusinessObject>, BusinessObjectPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<BusinessObjectInspector>, BusinessObjectInspectorPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<BusinessObjectInspection>, BusinessObjectInspectionPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<BusinessObjectInspectionAuditRecord>, BusinessObjectInspectionAuditRecordPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<Identity>, IdentityPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<Inspection>, InspectionPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<Inspector>, InspectorPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<IdentityInspector>, IdentityInspectorPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<Notification>, NotificationPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<Organization>, OrganizationPartitionKeyValueGenerator>();

        services.AddScoped<IPartitionKeyValueGenerator<Continuation>, ContinuationPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<Event>, EventPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<EventProcessing>, EventProcessingPartitionKeyValueGenerator>();
        services.AddScoped<IPartitionKeyValueGenerator<Worker>, WorkerPartitionKeyValueGenerator>();

        services.AddScoped<IPartitionKeyValueGenerator<Extension>, ExtensionPartitionKeyValueGenerator>();

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

    public static IServiceProvider ConfigureData(this IServiceProvider services, string? organization, string? inspector)
    {
        var state = services.GetRequiredService<PaulaContextState>();

        state.CurrentOrganization = organization != null
            ? $"{organization}"
            : string.Empty;

        state.CurrentInspector = inspector != null
            ? $"{inspector}"
            : string.Empty;

        return services
            .ConfigureExtensionModelIndicator(state);
    }

    public static IServiceProvider ConfigureData(this IServiceProvider services)
    {
        var user = services.GetRequiredService<ClaimsPrincipal>();
        var state = services.GetRequiredService<PaulaContextState>();

        state.CurrentOrganization = user.Claims.HasOrganization()
           ? user.Claims.GetOrganization()
           : string.Empty;

        state.CurrentInspector = user.Claims.HasInspector()
           ? user.Claims.GetInspector()
           : string.Empty;

        return services
            .ConfigureExtensionModelIndicator(state);
    }



    private static IServiceProvider ConfigureExtensionModelIndicator(this IServiceProvider services, PaulaContextState state)
    {
        if (!string.IsNullOrWhiteSpace(state.CurrentOrganization))
        {
            var extensionProvider = services.GetRequiredService<ExtensionProvider>();

            var extensionTags = ExtensionAggregateTypes.All
                    .Select(extensionProvider.Get)
                    .Where(x => x != null)
                    .Select(x => x!.ETag);

            var extensionModelIndicator = string.Join(string.Empty, extensionTags)
                .GetHashCode()
                .ToString();

            state.ExtensionModelIndicator = extensionModelIndicator;
        }

        return services;
    }
}