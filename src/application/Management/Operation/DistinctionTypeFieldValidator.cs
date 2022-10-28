using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public static class DistinctionTypeFieldValidator
{
    public static (bool, Func<(string, FormattableString)>) ExtensionFieldIsNotEmpty(string extensionField)
        => (!string.IsNullOrWhiteSpace(extensionField),
            () => (nameof(extensionField), $"Extension field can not be empty"));

    public static (bool, Func<(string, FormattableString)>) ExtensionFieldHasKebabCase(string extensionField)
        => (string.IsNullOrWhiteSpace(extensionField) || KebabCaseValidator.IsValid(extensionField),
            () => (nameof(extensionField), $"Extension field '{extensionField}' must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) ExtensionFieldIsNotTooLong(string extensionField)
        => (string.IsNullOrWhiteSpace(extensionField) || extensionField.Length <= 140,
            () => (nameof(extensionField), $"Extension field '{extensionField}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) ExtensionFieldHasValidValue(string extensionField)
        => (string.IsNullOrWhiteSpace(extensionField) || UniqueNameValidator.IsValid(extensionField),
            () => (nameof(extensionField), $"Extension field '{extensionField}' has an invalid value"));
}