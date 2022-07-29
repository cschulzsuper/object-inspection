using System.Collections.Generic;

namespace Super.Paula.Application.Setup
{
    public interface IExtensionTypeManager
    {
        IAsyncEnumerable<string> GetAsyncEnumerable();
    }
}