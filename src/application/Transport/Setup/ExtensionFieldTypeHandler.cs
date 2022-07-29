using System.Collections.Generic;

namespace Super.Paula.Application.Setup
{
    public class ExtensionFieldTypeHandler : IExtensionFieldTypeHandler
    {
        private readonly IExtensionFieldTypeManager _extensionFieldTypeManager;

        public ExtensionFieldTypeHandler(
            IExtensionFieldTypeManager extensionFieldTypeManager)
        {
            _extensionFieldTypeManager = extensionFieldTypeManager;
        }

        public IAsyncEnumerable<string> GetAll()
            => _extensionFieldTypeManager.GetAsyncEnumerable();
    }
}