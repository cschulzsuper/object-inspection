using System.Collections.Generic;

namespace Super.Paula.Application.Setup
{
    public class ExtensionTypeHandler : IExtensionTypeHandler
    {
        private readonly IExtensionTypeManager _extensionTypeManager;

        public ExtensionTypeHandler(
            IExtensionTypeManager extensionTypeManager)
        {
            _extensionTypeManager = extensionTypeManager;
        }

        public IAsyncEnumerable<string> GetAll()
            => _extensionTypeManager.GetAsyncEnumerable();
    }
}