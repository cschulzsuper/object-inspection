using System.Collections.Generic;

namespace Super.Paula.Application.Operation
{
    public interface IExtensionFieldTypeHandler
    {
        IAsyncEnumerable<string> GetAll();
    }
}