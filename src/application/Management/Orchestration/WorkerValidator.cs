using Super.Paula.Validation;
using System;

namespace Super.Paula.Application.Orchestration
{
    public static class WorkerValidator
    {
        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotEmpty(string uniqueName)
            => (!string.IsNullOrWhiteSpace(uniqueName),
                () => (nameof(uniqueName), $"Unique name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasKebabCase(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || KebabCaseValidator.IsValid(uniqueName),
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotTooLong(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || uniqueName.Length <= 140,
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasValidValue(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || UniqueNameValidator.IsValid(uniqueName),
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) IterationDelayIsInDayTimeRange(int iterationDelay)
            => (MillisecondsValidator.IsValid(iterationDelay),
                () => (nameof(iterationDelay), $"Iteration delay '{iterationDelay}' must be positive and less than 86400000"));
    }
}
