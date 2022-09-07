using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Authentication;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Operation;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerManagement(this IServiceCollection services)
    {
        services.AddServerManagementAdministration();
        services.AddServerManagementAuditing();
        services.AddServerManagementAuthentication();
        services.AddServerManagementCommunication();
        services.AddServerManagementGuidelines();
        services.AddServerManagementInventory();
        services.AddServerManagementOperation();
        services.AddServerManagementOrchestration();

        return services;
    }

    private static IServiceCollection AddServerManagementAdministration(this IServiceCollection services)
    {
        services.AddScoped<IInspectorManager, InspectorManager>();
        services.AddScoped<IIdentityInspectorManager, IdentityInspectorManager>();
        services.AddScoped<IOrganizationManager, OrganizationManager>();
        services.AddScoped<IInspectorAvatarManager, InspectorAvatarManager>();

        return services;
    }

    private static IServiceCollection AddServerManagementAuditing(this IServiceCollection services)
    {
        services.AddScoped<IBusinessObjectInspectionManager, BusinessObjectInspectionManager>();
        services.AddScoped<IBusinessObjectInspectionAuditRecordManager, BusinessObjectInspectionAuditRecordManager>();

        services.AddScoped<IBusinessObjectInspectorManager, BusinessObjectInspectorManager>();

        return services;
    }

    private static IServiceCollection AddServerManagementAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IIdentityManager, IdentityManager>();

        return services;
    }

    private static IServiceCollection AddServerManagementCommunication(this IServiceCollection services)
    {
        services.AddScoped<INotificationManager, NotificationManager>();

        return services;
    }

    private static IServiceCollection AddServerManagementGuidelines(this IServiceCollection services)
    {
        services.AddScoped<IInspectionManager, InspectionManager>();

        return services;
    }

    private static IServiceCollection AddServerManagementInventory(this IServiceCollection services)
    {
        services.AddScoped<IBusinessObjectManager, BusinessObjectManager>();

        return services;
    }

    private static IServiceCollection AddServerManagementOperation(this IServiceCollection services)
    {
        services.AddScoped<IApplicationManager, ApplicationManager>();
        services.AddScoped<IConnectionManager, ConnectionManager>();
        services.AddScoped<IConnectionViolationManager, ConnectionViolationManager>();

        return services;
    }

    private static IServiceCollection AddServerManagementOrchestration(this IServiceCollection services)
    {
        services.AddScoped<IContinuationManager, ContinuationManager>();
        services.AddScoped<IEventManager, EventManager>();
        services.AddScoped<IEventProcessingManager, EventProcessingManager>();
        services.AddScoped<IWorkerManager, WorkerManager>();
        services.AddScoped<IWorkerRuntimeManager, WorkerRuntimeManager>();

        services.AddScoped<IExtensionManager, ExtensionManager>();
        services.AddScoped<IExtensionAggregateTypeManager, ExtensionAggregateTypeManager>();
        services.AddScoped<IExtensionFieldTypeManager, ExtensionFieldTypeManager>();

        return services;
    }
}