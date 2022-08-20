using System.Collections.Generic;

namespace Super.Paula.Application.Operation;

public class ExtensionFieldTypeRequestHandler : IExtensionFieldTypeRequestHandler
{
    private readonly IExtensionFieldTypeManager _extensionFieldTypeManager;

    public ExtensionFieldTypeRequestHandler(
        IExtensionFieldTypeManager extensionFieldTypeManager)
    {
        _extensionFieldTypeManager = extensionFieldTypeManager;
    }

    public IAsyncEnumerable<string> GetAll()
        => _extensionFieldTypeManager.GetAsyncEnumerable();
}