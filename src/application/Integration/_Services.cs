using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerIntegration(this IServiceCollection services)
             => services
                .AddSingleton<IEventBus, LocalEventBus>()
                .AddScoped<ITokenAuthorizationFilter, TokenAuthorizationFilter>()
                .AddScoped<IBusinessObjectInspectionAuditScheduleFilter, BusinessObjectInspectionAuditScheduleFilter>();

    }
}
