using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Paula.Application;
using Super.Paula.Application.Orchestration;
using Super.Paula.Authorization;
using Super.Paula.Data;
using Super.Paula.Steps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula
{
    public class HostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;

        public HostedService(
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider)
        {
            _applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken _)
        {
            ConfigureEvents(_serviceProvider);

            await IStep.ExecuteAsync<BusinessObjectInspectionAuditScheduleCalculation>(_serviceProvider);

            _applicationLifetime.StopApplication();
        }

        public static IServiceProvider ConfigureEvents(IServiceProvider services)
        {
            var eventBus = services.GetRequiredService<IEventBus>();

            eventBus.Configure((context) =>
            {
                var paulaContextState = context.Services.GetRequiredService<PaulaContextState>();

                paulaContextState.CurrentOrganization = context.User.HasOrganization()
                   ? context.User.GetOrganization()
                   : string.Empty;

                paulaContextState.CurrentInspector = context.User.HasInspector()
                   ? context.User.GetInspector()
                   : string.Empty;
            });

            eventBus.ConfigureTransport();

            return services;
        }

        public Task StopAsync(CancellationToken _)
            => Task.CompletedTask;
    }
}
