using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public static class DistinctionTypeValidator
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

    public static (bool, Func<(string, FormattableString)>) FieldExtensionFieldsAreUnique(ICollection<DistinctionTypeField> fields)
        => (!fields.GroupBy(x => x.ExtensionField).Any(x => x.Count() > 1),
            () => (nameof(fields), $"Fields with extension field duplicates are not allowed in distinction type"));
}