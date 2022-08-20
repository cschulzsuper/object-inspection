using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auth;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Operation;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddServerTransport(this IServiceCollection services)
    {
        services.AddServerTransportAdministration();
        services.AddServerTransportAuditing();
        services.AddServerTransportAuth();
        services.AddServerTransportCommunication();
        services.AddServerTransportGuidelines();
        services.AddServerTransportInventory();
        services.AddServerTransportOperation();

        return services;
    }

    private static IServiceCollection AddServerTransportAdministration(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationRequestHandler, AuthorizationRequestHandler>();
        services.AddScoped<IAuthorizationTokenHandler, AuthorizationTokenHandler>();

        services.AddScoped<IInspectorRequestHandler, InspectorRequestHandler>();
        services.AddScoped<IInspectorEventService, InspectorEventService>();
        services.AddScoped<IInspectorContinuationService, InspectorContinuationService>();

        services.AddScoped<IInspectorAvatarRequestHandler, InspectorAvatarRequestHandler>();

        services.AddScoped<IOrganizationRequestHandler, OrganizationRequestHandler>();
        services.AddScoped<IOrganizationQueries, OrganizationQueries>();
        services.AddScoped<IOrganizationEventService, OrganizationEventService>();
        services.AddScoped<IOrganizationContinuationService, OrganizationContinuationService>();

        return services;
    }

    private static IServiceCollection AddServerTransportAuditing(this IServiceCollection services)
    {
        services.AddScoped<IBusinessObjectInspectionAuditRecordRequestHandler, BusinessObjectInspectionAuditRecordRequestHandler>();
        services.AddScoped<IBusinessObjectInspectionContinuationService, BusinessObjectInspectionContinuationService>();
        services.AddScoped<IBusinessObjectInspectionAuditScheduler, BusinessObjectInspectionAuditScheduler>();
        services.AddScoped<IBusinessObjectInspectionRequestHandler, BusinessObjectInspectionRequestHandler>();
        services.AddScoped<IBusinessObjectInspectionEventService, BusinessObjectInspectionEventService>();

        return services;
    }

    private static IServiceCollection AddServerTransportAuth(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<Identity>, IdentityPasswordHasher>();
        services.AddScoped<IIdentityRequestHandler, IdentityRequestHandler>();
        services.AddScoped<IAuthenticationRequestHandler, AuthenticationRequestHandler>();

        return services;
    }

    private static IServiceCollection AddServerTransportCommunication(this IServiceCollection services)
    {
        services.AddScoped<INotificationRequestHandler, NotificationRequestHandler>();

        return services;
    }

    private static IServiceCollection AddServerTransportGuidelines(this IServiceCollection services)
    {
        services.AddScoped<IInspectionRequestHandler, InspectionRequestHandler>();
        services.AddScoped<IInspectionEventService, InspectionEventService>();

        return services;
    }

    private static IServiceCollection AddServerTransportInventory(this IServiceCollection services)
    {
        services.AddScoped<IBusinessObjectRequestHandler, BusinessObjectRequestHandler>();
        services.AddScoped<IBusinessObjectQueries, BusinessObjectQueries>();
        services.AddScoped<IBusinessObjectEventService, BusinessObjectEventService>();

        return services;
    }

    private static IServiceCollection AddServerTransportOperation(this IServiceCollection services)
    {
        services.AddScoped<IExtensionAggregateTypeRequestHandler, ExtensionAggregateTypeRequestHandler>();
        services.AddScoped<IExtensionFieldTypeRequestHandler, ExtensionFieldTypeRequestHandler>();
        services.AddScoped<IExtensionRequestHandler, ExtensionRequestHandler>();

        return services;
    }
}