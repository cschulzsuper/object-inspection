using System;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory
{
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

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotEmpty(string displayName)
            => (!string.IsNullOrWhiteSpace(displayName),
                () => (nameof(displayName), $"Display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotTooLong(string displayName)
            => (string.IsNullOrWhiteSpace(displayName) || displayName.Length <= 140,
                () => (nameof(displayName), $"Display name '{displayName}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectorIsNotNull(string inspector)
            => (inspector != null,
                () => (nameof(inspector), $"Inspector can not be null"));

        public static (bool, Func<(string, FormattableString)>) InspectorHasKebabCase(string inspector)
            => (inspector == null || KebabCaseValidator.IsValid(inspector),
                () => (nameof(inspector), $"Inspector '{inspector}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) InspectorIsNotTooLong(string inspector)
            => (inspector == null || inspector.Length <= 140,
                () => (nameof(inspector), $"Inspector '{inspector}' can not have more than 140 characters"));
    }
}
