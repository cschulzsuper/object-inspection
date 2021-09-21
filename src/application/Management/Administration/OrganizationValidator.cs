using Super.Paula.Validation;
using System;
using System.Linq;

namespace Super.Paula.Application.Administration
{
    public static class OrganizationValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasValue(string uniqueName)
            => (_ => !string.IsNullOrWhiteSpace(uniqueName),
                    () => $"Unique name must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasKebabCase(string uniqueName)
            => (_ => KebabCaseValidator.IsValid(uniqueName),
                    () => $"Unique name '{uniqueName}' of organization must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) ChiefInspectorIsNotNull(Organization organization)
            => (_ => organization.ChiefInspector != null,
                    () => $"Chief inspector of organization '{organization.UniqueName}' can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) ChiefInspectorHasKebabCase(Organization organization)
            => (_ => KebabCaseValidator.IsValid(organization.ChiefInspector),
                    () => $"Chief inspector '{organization.ChiefInspector}' of organization '{organization.UniqueName}' must be in kebab case");

    }
}
