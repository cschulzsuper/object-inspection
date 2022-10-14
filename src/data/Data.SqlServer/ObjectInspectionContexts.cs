using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChristianSchulz.ObjectInspection.Data;

public class ObjectInspectionContexts
{
    private readonly IServiceProvider _serviceProvider;

    public ObjectInspectionContexts(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private AdministrationContext? _administration;
    private ApplicationContext? _application;
    private OperationContext? _operation;

    public AdministrationContext Administration =>
        _administration ??= _serviceProvider.GetRequiredService<AdministrationContext>();

    public ApplicationContext Application =>
        _application ??= _serviceProvider.GetRequiredService<ApplicationContext>();

    public OperationContext Operation =>
        _operation ??= _serviceProvider.GetRequiredService<OperationContext>();
}