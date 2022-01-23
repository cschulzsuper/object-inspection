using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Inventory;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerIntegration(this IServiceCollection services)
             => services
                 .AddScoped<ITokenAuthorizationFilter, TokenAuthorizationFilter>()
                 .AddScoped<IBusinessObjectInspectionAuditScheduleFilter, BusinessObjectInspectionAuditScheduleFilter>();

    }
}
