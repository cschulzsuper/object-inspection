using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

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