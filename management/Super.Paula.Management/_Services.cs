using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Management.Contract;

namespace Super.Paula.Management
{
    public static class _Services
    {
        public static IServiceCollection AddPaulaManagement(this IServiceCollection services)
            => services
                .AddScoped<IBusinessObjectManager, BusinessObjectManager>()
                .AddScoped<IInspectorManager, InspectorManager>()
                .AddScoped<IOrganizationManager, OrganizationManager>();
    }
}