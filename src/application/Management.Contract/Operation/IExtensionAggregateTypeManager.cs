using System.Collections.Generic;

namespace Super.Paula.Application.Operation
{
    public interface IExtensionAggregateTypeManager
    {
        IAsyncEnumerable<string> GetAsyncEnumerable();
    }
}