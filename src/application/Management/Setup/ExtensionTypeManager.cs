using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Setup
{
    public class ExtensionTypeManager : IExtensionTypeManager
    {
        public async IAsyncEnumerable<string> GetAsyncEnumerable()
        {
            await ValueTask.CompletedTask;

            foreach(var extensionType in ExtensionTypes.All)
            {
                yield return extensionType;
            }
        }
    }
}