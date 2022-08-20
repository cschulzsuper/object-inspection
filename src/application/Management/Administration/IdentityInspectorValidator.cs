using Super.Paula.Shared.Validation;
using System;

namespace Super.Paula.Application.Administration;

public static class IdentityInspectorValidator
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

    public static (bool, Func<(string, FormattableString)>) OrganizationIsNotEmpty(string organization)
        => (!string.IsNullOrWhiteSpace(organization),
            () => (nameof(organization), $"Organization can not be empty"));

    public static (bool, Func<(string, FormattableString)>) OrganizationHasKebabCase(string organization)
        => (string.IsNullOrWhiteSpace(organization) || KebabCaseValidator.IsValid(organization),
            () => (nameof(organization), $"Organization '{organization}' must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) OrganizationIsNotTooLong(string organization)
        => (string.IsNullOrWhiteSpace(organization) || organization.Length <= 140,
            () => (nameof(organization), $"Organization '{organization}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) OrganizationHasValidValue(string organization)
        => (string.IsNullOrWhiteSpace(organization) || UniqueNameValidator.IsValid(organization),
            () => (nameof(organization), $"Organization '{organization}' has an invalid value"));

    public static (bool, Func<(string, FormattableString)>) InspectorIsNotEmpty(string inspector)
        => (!string.IsNullOrWhiteSpace(inspector),
            () => (nameof(inspector), $"Inspector can not be empty"));

    public static (bool, Func<(string, FormattableString)>) InspectorHasKebabCase(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || KebabCaseValidator.IsValid(inspector),
            () => (nameof(inspector), $"Inspector '{inspector}' must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) InspectorIsNotTooLong(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || inspector.Length <= 140,
            () => (nameof(inspector), $"Inspector '{inspector}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) InspectorHasValidValue(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || UniqueNameValidator.IsValid(inspector),
            () => (nameof(inspector), $"Inspector '{inspector}' has an invalid value"));

}