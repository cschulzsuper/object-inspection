using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public static class ExtensionFieldValidator
{
    public static (bool, Func<(string, FormattableString)>) DataTypeIsNotEmpty(string dataType)
        => (!string.IsNullOrWhiteSpace(dataType),
            () => (nameof(dataType), $"Data type can not be empty"));

    public static (bool, Func<(string, FormattableString)>) DataTypeHasLowerCase(string dataType)
        => (string.IsNullOrWhiteSpace(dataType) || LowerCaseValidator.IsValid(dataType),
            () => (nameof(dataType), $"Data type '{dataType}' must have lower case"));

    public static (bool, Func<(string, FormattableString)>) DataTypeIsNotTooLong(string dataType)
        => (string.IsNullOrWhiteSpace(dataType) || dataType.Length <= 140,
            () => (nameof(dataType), $"Data type '{dataType}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) DataTypeHasValidValue(string dataType)
        => (string.IsNullOrWhiteSpace(dataType) || ExtensionFieldDataTypeValidator.IsValid(dataType),
            () => (nameof(dataType), $"Data type '{dataType}' has an invalid value"));

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




    public static (bool, Func<(string, FormattableString)>) DataNameIsNotEmpty(string dataName)
        => (!string.IsNullOrWhiteSpace(dataName),
            () => (nameof(dataName), $"Data name can not be empty"));

    public static (bool, Func<(string, FormattableString)>) DataNameHasCamelCase(string dataName)
        => (string.IsNullOrWhiteSpace(dataName) || CamelCaseValidator.IsValid(dataName),
            () => (nameof(dataName), $"Data name '{dataName}' must have camel case"));

    public static (bool, Func<(string, FormattableString)>) DataNameIsNotTooLong(string dataName)
        => (string.IsNullOrWhiteSpace(dataName) || dataName.Length <= 140,
            () => (nameof(dataName), $"Data name '{dataName}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) DataNameHasValidValue(string dataName)
        => (string.IsNullOrWhiteSpace(dataName) || ExtensionFieldDataNameValidator.IsValid(dataName),
            () => (nameof(dataName), $"Data name '{dataName}' has an invalid value"));

}