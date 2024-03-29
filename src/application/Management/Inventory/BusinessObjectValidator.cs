﻿using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

public static class BusinessObjectValidator
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

    public static (bool, Func<(string, FormattableString)>) DistinctionTypeIsNotNull(string distinctionType)
    => (distinctionType != null,
        () => (nameof(distinctionType), $"Distinction type can not be null"));

    public static (bool, Func<(string, FormattableString)>) DistinctionTypeHasKebabCase(string distinctionType)
        => (string.IsNullOrWhiteSpace(distinctionType) || KebabCaseValidator.IsValid(distinctionType),
            () => (nameof(distinctionType), $"Distinction type '{distinctionType}' must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) DistinctionTypeIsNotTooLong(string distinctionType)
        => (string.IsNullOrWhiteSpace(distinctionType) || distinctionType.Length <= 140,
            () => (nameof(distinctionType), $"Distinction type '{distinctionType}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) DistinctionTypeHasValidValue(string distinctionType)
        => (string.IsNullOrWhiteSpace(distinctionType) || UniqueNameValidator.IsValid(distinctionType),
            () => (nameof(distinctionType), $"Distinction type '{distinctionType}' has an invalid value"));

    public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotEmpty(string displayName)
        => (!string.IsNullOrWhiteSpace(displayName),
            () => (nameof(displayName), $"Display name can not be empty"));

    public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotTooLong(string displayName)
        => (string.IsNullOrWhiteSpace(displayName) || displayName.Length <= 140,
            () => (nameof(displayName), $"Display name '{displayName}' can not have more than 140 characters"));
}