using System.Collections.Generic;

namespace Super.Paula.Application.Setup
{
    public interface IExtensionTypeHandler
    {
        IAsyncEnumerable<string> GetAll();
    }
}