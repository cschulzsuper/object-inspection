using System.Collections.Generic;

namespace Super.Paula.Application.Operation
{
    public interface IExtensionTypeHandler
    {
        IAsyncEnumerable<string> GetAll();
    }
}