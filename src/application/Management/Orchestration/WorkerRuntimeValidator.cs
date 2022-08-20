using Super.Paula.Shared.Validation;
using System;

namespace Super.Paula.Application.Orchestration;

public static class WorkerRuntimeValidator
{
    public static (bool, Func<(string, FormattableString)>) StateIsNotNull(string state)
        => (state != null,
            () => (nameof(state), $"State can not be null"));

    public static (bool, Func<(string, FormattableString)>) StateHasValidValue(string state)
        => (state == null || WorkerStateValidator.IsValid(state),
            () => (nameof(state), $"State '{state}' is not a valid value"));
}