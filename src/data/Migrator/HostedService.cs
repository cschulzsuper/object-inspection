using Microsoft.Extensions.Hosting;
using Super.Paula.Data.Steps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Data
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
            await IStep.ExecuteAsync<Initialization>(_serviceProvider);

            await IStep.ExecuteAsync<InitializationApplication>(_serviceProvider);
            await IStep.ExecuteAsync<InspectorIdentity>(_serviceProvider);

            _applicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken _)
            => Task.CompletedTask;
    }
}
