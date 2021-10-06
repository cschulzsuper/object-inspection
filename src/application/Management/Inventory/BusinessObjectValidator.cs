using System;
using System.Linq;
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

        public static (bool, Func<(string, FormattableString)>) InspectionUniqueNameIsNotEmpty(string inspectionUniqueName)
            => (!string.IsNullOrWhiteSpace(inspectionUniqueName),
                () => (nameof(inspectionUniqueName), $"Unique name of inspection can not be empty"));

        public static (bool, Func<(string, FormattableString)>) InspectionUniqueNameHasKebabCase(string inspectionUniqueName)
            => (string.IsNullOrWhiteSpace(inspectionUniqueName) || KebabCaseValidator.IsValid(inspectionUniqueName),
                () => (nameof(inspectionUniqueName), $"Unique name '{inspectionUniqueName}' of inspection must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) InspectionUniqueNameIsNotTooLong(string inspectionUniqueName)
            => (string.IsNullOrWhiteSpace(inspectionUniqueName) || inspectionUniqueName.Length <= 140,
                () => (nameof(inspectionUniqueName), $"Unique name '{inspectionUniqueName}' of inspection can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectionDisplayNameIsNotEmpty(string inspectionDisplayName)
            => (!string.IsNullOrWhiteSpace(inspectionDisplayName),
                () => (nameof(inspectionDisplayName), $"Display name of inspection can not be empty"));

        public static (bool, Func<(string, FormattableString)>) InspectionDisplayNameIsNotTooLong(string inspectionDisplayName)
            => (string.IsNullOrWhiteSpace(inspectionDisplayName) || inspectionDisplayName.Length <= 140,
                () => (nameof(inspectionDisplayName), $"Display name '{inspectionDisplayName}' of inspection can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectionTextIsNotNull(string inspectionText)
            => (inspectionText != null,
                () => (nameof(inspectionText), $"Text of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) InspectionTextIsNotTooLong(string inspectionText)
            => (inspectionText == null || inspectionText.Length <= 4000,
                () => (nameof(inspectionText), $"Text '{inspectionText}' of inspection can not have more than 4000 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditInspectorIsNotNull(string inspectionAuditInspector)
            => (inspectionAuditInspector != null,
                () => (nameof(inspectionAuditInspector), $"Audit inspector '{inspectionAuditInspector}' of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditInspectorHasKebabCase(string inspectionAuditInspector)
            => (inspectionAuditInspector == null || KebabCaseValidator.IsValid(inspectionAuditInspector),
                () => (nameof(inspectionAuditInspector), $"Audit inspector '{inspectionAuditInspector}' of inspection must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditInspectorIsNotTooLong(string inspectionAuditInspector)
            => (inspectionAuditInspector == null || inspectionAuditInspector.Length <= 140,
                () => (nameof(inspectionAuditInspector), $"Audit inspector '{inspectionAuditInspector}' of inspection can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditAnnotationIsNotNull(string inspectionAuditAnnotation)
            => (inspectionAuditAnnotation != null,
                () => (nameof(inspectionAuditAnnotation), $"Audit annotation of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditAnnotationIsNotTooLong(string inspectionAuditAnnotation)
            => (inspectionAuditAnnotation == null || inspectionAuditAnnotation.Length <= 4000,
                () => (nameof(inspectionAuditAnnotation), $"Audit annotation '{inspectionAuditAnnotation}' of inspection can not have more than 4000 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditResultIsNotNull(string inspectionAuditResult)
            => (inspectionAuditResult != null,
                () => (nameof(inspectionAuditResult), $"Audit result of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditResultHasValidValue(string inspectionAuditResult)
            => (inspectionAuditResult == null || AuditResultValidator.IsValid(inspectionAuditResult),
                () => (nameof(inspectionAuditResult), $"Audit result '{inspectionAuditResult}' of inspection is not a valid value"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditDateIsPositive(int inspectionAuditDate)
            => (DayNumberValidator.IsValid(inspectionAuditDate),
                () => (nameof(inspectionAuditDate), $"Audit date '{inspectionAuditDate}' of inspection must be positive"));

        public static (bool, Func<(string, FormattableString)>) InspectionAuditTimeIsInDayTimeRange(int inspectionAuditTime)
            => (MillisecondsValidator.IsValid(inspectionAuditTime),
                () => (nameof(inspectionAuditTime), $"Audit time '{inspectionAuditTime}' of inspection must be positive and less than 86400000"));
    }
}
