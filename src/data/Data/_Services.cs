using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using ChristianSchulz.ObjectInspection.Application.Communication;
using ChristianSchulz.ObjectInspection.Application.Guidelines;
using ChristianSchulz.ObjectInspection.Application.Inventory;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using ChristianSchulz.ObjectInspection.Data.Mappings;
using ChristianSchulz.ObjectInspection.Data.Mappings.Administration;
using ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;
using ChristianSchulz.ObjectInspection.Data.Mappings.Communication;
using ChristianSchulz.ObjectInspection.Data.Mappings.Guidelines;
using ChristianSchulz.ObjectInspection.Data.Mappings.Inventory;
using ChristianSchulz.ObjectInspection.Data.Mappings.Operation;
using ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using ChristianSchulz.ObjectInspection.Application.Authentication;
using ChristianSchulz.ObjectInspection.Data.Mappings.Authentication;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Data;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerData(this IServiceCollection services, bool isDevelopment)
    {
        services.AddDbContext<AdministrationContext>((services, options) =>
        {
            var appSeetings = services.GetRequiredService<AppSettings>();

            options.ReplaceService<IModelCacheKeyFactory, AdministrationContextModelCacheKeyFactory>();

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

        services.AddDbContext<ApplicationContext>((services, options) =>
        {
            var appSeetings = services.GetRequiredService<AppSettings>();

            options.ReplaceService<IModelCacheKeyFactory, ApplicationContextModelCacheKeyFactory>();

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

        services.AddDbContext<OperationContext>((services, options) =>
        {
            var appSettings = services.GetRequiredService<AppSettings>();

            options.ReplaceService<IModelCacheKeyFactory, OperationContextModelCacheKeyFactory>();

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


        services.AddScoped<ObjectInspectionContexts>();
        services.AddScoped<ObjectInspectionContextState>();

        services.AddScoped<ExtensionProvider>();
        services.AddScoped<ExtensionCacheKeyFactory>();
        services.AddSingleton<ExtensionCache>();

        services.AddScoped<IRepositoryCreator, RepositoryCreator>();

        services.AddScoped(RepositoryFactory<Identity, AdministrationContext>());
        services.AddScoped(RepositoryFactory<IdentityInspector, AdministrationContext>());
        services.AddScoped(RepositoryFactory<Organization, AdministrationContext>());

        services.AddScoped<IRepository<Extension>, ExtensionRepository>();

        services.AddScoped(RepositoryFactory<Continuation, AdministrationContext>());
        services.AddScoped(RepositoryFactory<Event, AdministrationContext>());
        services.AddScoped(RepositoryFactory<EventProcessing, AdministrationContext>());
        services.AddScoped(RepositoryFactory<Worker, AdministrationContext>());

        services.AddScoped(RepositoryFactory<BusinessObject, ApplicationContext>());
        services.AddScoped(RepositoryFactory<BusinessObjectInspector, ApplicationContext>());
        services.AddScoped(RepositoryFactory<BusinessObjectInspection, ApplicationContext>());
        services.AddScoped(RepositoryFactory<BusinessObjectInspectionAuditRecord, ApplicationContext>());
        services.AddScoped(RepositoryFactory<Inspection, ApplicationContext>());
        services.AddScoped(RepositoryFactory<Inspector, ApplicationContext>());
        services.AddScoped(RepositoryFactory<Notification, ApplicationContext>());

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
        where TContext : ObjectInspectionContext
        where TEntity : class
        => services =>
            new Repository<TEntity>(
                services.GetRequiredService<TContext>(),
                services.GetRequiredService<ObjectInspectionContextState>(),
                services.GetRequiredService<IPartitionKeyValueGenerator<TEntity>>());

    public static IServiceProvider ConfigureData(this IServiceProvider services, string? organization, string? inspector)
    {
        var state = services.GetRequiredService<ObjectInspectionContextState>();

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
        var state = services.GetRequiredService<ObjectInspectionContextState>();

        state.CurrentOrganization = user.Claims.HasOrganization()
           ? user.Claims.GetOrganization()
           : string.Empty;

        state.CurrentInspector = user.Claims.HasInspector()
           ? user.Claims.GetInspector()
           : string.Empty;

        return services
            .ConfigureExtensionModelIndicator(state);
    }



    private static IServiceProvider ConfigureExtensionModelIndicator(this IServiceProvider services, ObjectInspectionContextState state)
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