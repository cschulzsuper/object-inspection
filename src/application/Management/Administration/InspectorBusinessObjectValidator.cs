using System;
using System.Collections.Generic;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Administration
{
    public static class InspectorBusinessObjectValidator
    {
        public static (bool, Func<(string, FormattableString)>) BusinessObjectsUnique(ICollection<InspectorBusinessObject> businessObject)
            => (!businessObject.GroupBy(x => x.UniqueName).Any(x => x.Count() > 1),
                () => (nameof(businessObject), $"business object duplicates are not allowed in inspector"));

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

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotEmpty(string displayName)
            => (!string.IsNullOrWhiteSpace(displayName),
                () => (nameof(displayName), $"Display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotTooLong(string displayName)
            => (string.IsNullOrWhiteSpace(displayName) || displayName.Length <= 140,
                () => (nameof(displayName), $"Display name '{displayName}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) AuditSchedulePlannedAuditDateIsPositive(int auditSchedulePlannedAuditDate)
            => (DayNumberValidator.IsValid(auditSchedulePlannedAuditDate),
                () => (nameof(auditSchedulePlannedAuditDate), $"Audit schedule planned audit date '{auditSchedulePlannedAuditDate}' of inspection must be positive"));

        public static (bool, Func<(string, FormattableString)>) AuditSchedulePlannedAuditTimeIsInDayTimeRange(int auditSchedulePlannedAuditTime)
            => (MillisecondsValidator.IsValid(auditSchedulePlannedAuditTime),
                () => (nameof(auditSchedulePlannedAuditTime), $"Audit schedule planned audit time '{auditSchedulePlannedAuditTime}' of inspection must be positive and less than 86400000"));

    }
}
