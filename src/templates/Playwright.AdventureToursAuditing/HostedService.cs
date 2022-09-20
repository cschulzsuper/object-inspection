using Microsoft.Extensions.Hosting;
using ChristianSchulz.ObjectInspection.Templates.Playwright.AdventureToursAuditing.Steps;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.AdventureToursAuditing;

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
        await IStep.ExecuteAsync<Steps.AdventureToursAuditing>(_serviceProvider);

        _applicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken _)
        => Task.CompletedTask;
}