using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Reflection;

namespace ChristianSchulz.ObjectInspection.Shared.SignalR;

public class HubContextResolver
{
    private readonly IServiceProvider serviceProvider;

    public HubContextResolver(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IHubContext GetHubContext(string name)
    {
        var hubType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(Hub)))
            .FirstOrDefault(t => t.GetCustomAttribute<HubNameAttribute>(true)?.Name == name);

        var hubContextType = typeof(IHubContext<>).MakeGenericType(hubType!);

        return (serviceProvider.GetService(hubContextType) as IHubContext)!;
    }
}