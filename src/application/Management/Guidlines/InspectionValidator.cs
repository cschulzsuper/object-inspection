using System;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Guidlines
{
    public static class InspectionValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasValue(string uniqueName)
            => (_ => !string.IsNullOrWhiteSpace(uniqueName),
                    () => $"Unique name of inspection must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasKebabCase(string uniqueName)
            => (_ => KebabCaseValidator.IsValid(uniqueName),
                    () => $"Unique name '{uniqueName}' of inspection must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) DisplayNameHasValue(Inspection inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection.DisplayName),
                    () => $"Display name of inspection '{inspection.UniqueName}' must have a value");
    }
}
