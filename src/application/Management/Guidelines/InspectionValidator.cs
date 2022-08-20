using Super.Paula.Shared.Validation;
using System;

namespace Super.Paula.Application.Guidelines;

public static class InspectionValidator
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

    public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotEmpty(string displayName)
        => (!string.IsNullOrWhiteSpace(displayName),
            () => (nameof(displayName), $"Display name can not be empty"));

    public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotTooLong(string displayName)
        => (string.IsNullOrWhiteSpace(displayName) || displayName.Length <= 140,
            () => (nameof(displayName), $"Display name '{displayName}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) TextIsNotNull(string text)
        => (text != null,
            () => (nameof(text), $"Text can not be null"));

    public static (bool, Func<(string, FormattableString)>) TextIsNotTooLong(string text)
        => (text == null || text.Length <= 4000,
            () => (nameof(text), $"Text '{text}' can not have more than 4000 characters"));
}