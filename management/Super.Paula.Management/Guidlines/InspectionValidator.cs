using System;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Guidlines
{
    public static class InspectionValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) InspectionHasValue(string inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection),
                    () => $"Inspection must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionHasKebabCase(string inspection)
            => (x => x && KebabCaseValidator.IsValid(inspection),
                    () => $"Inspection '{inspection}' must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionExists(string inspection, IQueryable<Inspection> inspections)
            => (x => x && inspections.FirstOrDefault(x => x.UniqueName == inspection) != null,
                    () => $"Inspection '{inspection}' does not exist");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasValue(Inspection inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection.UniqueName),
                    () => $"Unique name of inspection must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasKebabCase(Inspection inspection)
            => (x => x && KebabCaseValidator.IsValid(inspection.UniqueName),
                    () => $"Unique name '{inspection.UniqueName}' of inspection must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameIsUnqiue(Inspection inspection, IQueryable<Inspection> inspections)
            => (x => x && inspections.FirstOrDefault(x => x.UniqueName == inspection.UniqueName) == null,
                    () => $"Unique name '{inspection.UniqueName}' of inspection already exists");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameExists(Inspection inspection, IQueryable<Inspection> inspections)
            => (x => x && inspections.FirstOrDefault(x => x.UniqueName == inspection.UniqueName) != null,
                    () => $"Unique name '{inspection.UniqueName}' of inspection does not exist");

        public static (Func<bool, bool>, Func<FormattableString>) DisplayNameHasValue(Inspection inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection.DisplayName),
                    () => $"Display name of inspection '{inspection.UniqueName}' must have a value");
    }
}
