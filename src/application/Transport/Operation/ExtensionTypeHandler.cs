using System.Collections.Generic;

namespace Super.Paula.Application.Operation
{
    public class ExtensionTypeHandler : IExtensionTypeHandler
    {
        private readonly IExtensionAggregateTypeManager _extensionTypeManager;

        public ExtensionTypeHandler(
            IExtensionAggregateTypeManager extensionTypeManager)
        {
            _extensionTypeManager = extensionTypeManager;
        }

        public IAsyncEnumerable<string> GetAll()
            => _extensionTypeManager.GetAsyncEnumerable();
    }
}