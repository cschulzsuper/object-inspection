using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IExtensionFieldTypeRequestHandler
{
    IAsyncEnumerable<string> GetAll();
}