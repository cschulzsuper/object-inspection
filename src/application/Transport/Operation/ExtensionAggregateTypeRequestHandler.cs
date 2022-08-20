using System.Collections.Generic;

namespace Super.Paula.Application.Operation;

public class ExtensionAggregateTypeRequestHandler : IExtensionAggregateTypeRequestHandler
{
    private readonly IExtensionAggregateTypeManager _extensionAggregateTypeManager;

    public ExtensionAggregateTypeRequestHandler(IExtensionAggregateTypeManager extensionAggregateTypeManager)
    {
        _extensionAggregateTypeManager = extensionAggregateTypeManager;
    }

    public IAsyncEnumerable<string> GetAll()
        => _extensionAggregateTypeManager.GetAsyncEnumerable();
}