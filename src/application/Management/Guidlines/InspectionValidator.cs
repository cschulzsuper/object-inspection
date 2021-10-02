using System;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Guidlines
{
    public static class InspectionValidator
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

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotEmpty(string displayName)
            => (!string.IsNullOrWhiteSpace(displayName),
                () => (nameof(displayName), $"Display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotTooLong(string displayName)
            => (displayName == null || displayName.Length <= 140,
                () => (nameof(displayName), $"Display name can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) TextIsNotNull(string text)
            => (text != null,
                () => (nameof(text), $"Text can not be null"));

        public static (bool, Func<(string, FormattableString)>) TextIsNotTooLong(string text)
            => (text == null || text.Length <= 4000,
                () => (nameof(text), $"Text can not have more than 4000 characters"));
    }
}
