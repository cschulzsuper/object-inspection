using System;
using Super.Paula.Validation;

namespace Super.Paula.Application.Administration
{
    public static class InspectorValidator
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

        public static (bool, Func<(string, FormattableString)>) IdentityIsNotNull(string identity)
            => (identity != null,
                () => (nameof(identity), $"Identity can not be null"));

        public static (bool, Func<(string, FormattableString)>) IdentityIsNotTooLong(string identity)
            => (identity == null || identity.Length <= 140,
                () => (nameof(identity), $"Identity '{identity}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) OrganizationIsNotEmpty(string organization)
            => (!string.IsNullOrWhiteSpace(organization),
                () => (nameof(organization), $"Organization can not be empty"));

        public static (bool, Func<(string, FormattableString)>) OrganizationHasKebabCase(string organization)
            => (string.IsNullOrWhiteSpace(organization) || KebabCaseValidator.IsValid(organization),
                () => (nameof(organization), $"Organization '{organization}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) OrganizationIsNotTooLong(string organization)
            => (string.IsNullOrWhiteSpace(organization) || organization.Length <= 140,
                () => (nameof(organization), $"Organization '{organization}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) OrganizationDisplayNameIsNotEmpty(string organizationDisplayName)
            => (!string.IsNullOrWhiteSpace(organizationDisplayName),
                () => (nameof(organizationDisplayName), $"Organization display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) OrganizationDisplayNameIsNotTooLong(string organizationDisplayName)
            => (string.IsNullOrWhiteSpace(organizationDisplayName) || organizationDisplayName.Length <= 140,
                () => (nameof(organizationDisplayName), $"Organization display name '{organizationDisplayName}' can not have more than 140 characters"));
    }
}
