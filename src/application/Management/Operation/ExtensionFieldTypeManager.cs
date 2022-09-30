using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class ExtensionFieldTypeManager : IExtensionFieldTypeManager
{
    public async IAsyncEnumerable<string> GetAsyncEnumerable()
    {
        await ValueTask.CompletedTask;

        foreach (var extensionType in ExtensionFieldDataTypes.All)
        {
            yield return extensionType;
        }
    }
}