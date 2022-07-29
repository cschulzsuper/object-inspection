using System.Collections.Generic;

namespace Super.Paula.Application.Setup
{
    public interface IExtensionFieldTypeManager
    {
        IAsyncEnumerable<string> GetAsyncEnumerable();
    }
}