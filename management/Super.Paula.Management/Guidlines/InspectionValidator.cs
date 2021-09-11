using Super.Paula.Aggregates.Administration;
using Super.Paula.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Super.Paula.Aggregates.Guidlines;

namespace Super.Paula.Management.Guidlines
{
    public static class InspectionValidator
    {
        public static (Func<bool, bool>, FormattableString) InspectionHasValue(string inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection),
                    $"Inspection must have a value");

        public static (Func<bool, bool>, FormattableString) InspectionHasKebabCase(string inspection)
            => (x => x && KebabCaseValidator.IsValid(inspection),
                    $"Inspection '{inspection}' must be in kebab case");

        public static (Func<bool, bool>, FormattableString) InspectionExists(string inspection, IQueryable<Inspection> inspections)
            => (x => x && inspections.FirstOrDefault(x => x.UniqueName == inspection) != null,
                    $"Inspection '{inspection}' does not exist");

        public static (Func<bool, bool>, FormattableString) UniqueNameHasValue(Inspection inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection.UniqueName),
                    $"Unique name of inspection must have a value");

        public static (Func<bool, bool>, FormattableString) UniqueNameHasKebabCase(Inspection inspection)
            => (x => x && KebabCaseValidator.IsValid(inspection.UniqueName),
                    $"Unique name '{inspection.UniqueName}' of inspection must be in kebab case");

        public static (Func<bool, bool>, FormattableString) UniqueNameIsUnqiue(Inspection inspection, IQueryable<Inspection> inspections)
            => (x => x && inspections.FirstOrDefault(x => x.UniqueName == inspection.UniqueName) == null,
                    $"Unique name '{inspection.UniqueName}' of inspection already exists");

        public static (Func<bool, bool>, FormattableString) UniqueNameExists(Inspection inspection, IQueryable<Inspection> inspections)
            => (x => x && inspections.FirstOrDefault(x => x.UniqueName == inspection.UniqueName) != null,
                    $"Unique name '{inspection.UniqueName}' of inspection does not exist");

        public static (Func<bool, bool>, FormattableString) DisplayNameHasValue(Inspection inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection.DisplayName),
                    $"Display name of inspection '{inspection.UniqueName}' must have a value");
    }
}
