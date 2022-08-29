using Super.Paula.Application.Operation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Super.Paula.Data;

public class ExtensionCacheKeyFactory
{
    private readonly PaulaContextState _state;

    public ExtensionCacheKeyFactory(PaulaContextState state)
    {
        _state = state;
    }

    public string Create(string aggregateType)
        => $"{_state.CurrentOrganization}|{aggregateType}";
}