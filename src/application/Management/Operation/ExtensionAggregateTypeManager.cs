﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class ExtensionAggregateTypeManager : IExtensionAggregateTypeManager
{
    public async IAsyncEnumerable<string> GetAsyncEnumerable()
    {
        await ValueTask.CompletedTask;

        foreach (var extensionType in ExtensionAggregateTypes.All)
        {
            yield return extensionType;
        }
    }
}