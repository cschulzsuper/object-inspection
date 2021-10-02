using Super.Paula.Validation;
using System;
using System.Linq;

namespace Super.Paula.Application.Administration
{
    public static class OrganizationValidator
    {
        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotEmpty(string uniqueName)
            => (!string.IsNullOrWhiteSpace(uniqueName),
                () => (nameof(uniqueName), $"Unique name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasKebabCase(string uniqueName)
            => (KebabCaseValidator.IsValid(uniqueName),
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' must be in kebab case"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotTooLong(string uniqueName)
            => (uniqueName == null || uniqueName.Length <= 140,
                () => (nameof(uniqueName), $"Unique name can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) DisplayNameHasValue(string displayName)
            => (!string.IsNullOrWhiteSpace(displayName),
                () => (nameof(displayName), $"Display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotTooLong(string displayName)
            => (displayName == null || displayName.Length <= 140,
                () => (nameof(displayName), $"Display name can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) ChiefInspectorIsNotEmpty(string chiefInspector)
            => (!string.IsNullOrWhiteSpace(chiefInspector),
                () => (nameof(chiefInspector), $"Chief inspector can not be empty"));

        public static (bool, Func<(string, FormattableString)>) ChiefInspectorHasKebabCase(string chiefInspector)
            => (KebabCaseValidator.IsValid(chiefInspector),
                () => (nameof(chiefInspector), $"Chief inspector '{chiefInspector}' must be in kebab case"));

        public static (bool, Func<(string, FormattableString)>) ChiefInspectorIsNotTooLong(string chiefInspector)
            => (chiefInspector == null || chiefInspector.Length <= 140,
                () => (nameof(chiefInspector), $"Chief inspector can not have more than 140 characters"));
    }
}
