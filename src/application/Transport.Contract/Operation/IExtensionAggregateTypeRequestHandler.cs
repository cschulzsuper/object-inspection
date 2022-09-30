using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IExtensionAggregateTypeRequestHandler
{
    IAsyncEnumerable<string> GetAll();
}