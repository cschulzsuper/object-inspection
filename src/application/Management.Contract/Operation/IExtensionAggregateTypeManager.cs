using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IExtensionAggregateTypeManager
{
    IAsyncEnumerable<string> GetAsyncEnumerable();
}