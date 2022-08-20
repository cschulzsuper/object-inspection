using System.Collections.Generic;

namespace Super.Paula.Application.Operation;

public interface IExtensionAggregateTypeRequestHandler
{
    IAsyncEnumerable<string> GetAll();
}