using Microsoft.Extensions.Configuration;

namespace ChristianSchulz.ObjectInspection.Client.Configuration;

public class BlazorApiConfigurationSource : IConfigurationSource
{
    private readonly IConfigurationProvider _provider;

    public BlazorApiConfigurationSource(IConfigurationProvider provider) =>
        _provider = provider;

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        _provider;


}