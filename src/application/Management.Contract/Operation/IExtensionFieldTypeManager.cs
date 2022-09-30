using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IExtensionFieldTypeManager
{
    IAsyncEnumerable<string> GetAsyncEnumerable();
}