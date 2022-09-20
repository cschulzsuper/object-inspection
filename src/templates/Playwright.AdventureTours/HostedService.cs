using Microsoft.Extensions.Hosting;
using ChristianSchulz.ObjectInspection.Templates.Playwright.AdventureTours.Steps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.AdventureTours;

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
        // await IStep.ExecuteAsync<Purge>(_serviceProvider);
        // await IStep.ExecuteAsync<Initialization>(_serviceProvider);

        await IStep.ExecuteAsync<InitializationAdministration>(_serviceProvider);
        await IStep.ExecuteAsync<InitializationAdventureTours>(_serviceProvider);

        _applicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken _)
        => Task.CompletedTask;
}