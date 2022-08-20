using System.Collections.Generic;

namespace Super.Paula.Application.Operation;

public interface IExtensionFieldTypeManager
{
    IAsyncEnumerable<string> GetAsyncEnumerable();
}