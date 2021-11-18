using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Migrator;
using Migrator.Steps;
using Super.Paula.Environment;

namespace Super.Playground.Data.Migrator
{
    public class HostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;

        public HostedService(
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            AppSettings appSettings)
        {
            _applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _appSettings = appSettings;
        }

        public async Task StartAsync(CancellationToken _)
        {
            _appSettings.CosmosEndpoint = _configuration["CosmosEndpoint"];
            _appSettings.CosmosKey = _configuration["CosmosKey"];

            await IStep.ExecuteAsync<Initialization>(_serviceProvider);

            _applicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken _)
            => Task.CompletedTask;
    }
}
