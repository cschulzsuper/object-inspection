using Microsoft.Extensions.DependencyInjection;
using System;

namespace Super.Paula.Data;

public class PaulaContexts
{
    private readonly IServiceProvider _serviceProvider;

    public PaulaContexts(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private PaulaAdministrationContext? _administration;
    private PaulaApplicationContext? _application;
    private PaulaOperationContext? _operation;

    public PaulaAdministrationContext Administration =>
        _administration ??= _serviceProvider.GetRequiredService<PaulaAdministrationContext>();

    public PaulaApplicationContext Application =>
        _application ??= _serviceProvider.GetRequiredService<PaulaApplicationContext>();

    public PaulaOperationContext Operation =>
        _operation ??= _serviceProvider.GetRequiredService<PaulaOperationContext>();
}