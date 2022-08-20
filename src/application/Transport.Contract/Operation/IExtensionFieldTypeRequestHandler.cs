using System.Collections.Generic;

namespace Super.Paula.Application.Operation;

public interface IExtensionFieldTypeRequestHandler
{
    IAsyncEnumerable<string> GetAll();
}