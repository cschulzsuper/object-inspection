using System;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory
{
    public static class BusinessObjectInspectionValidator
    {
        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotEmpty(string uniqueName)
            => (!string.IsNullOrWhiteSpace(uniqueName),
                () => (nameof(uniqueName), $"Unique name of inspection can not be empty"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasKebabCase(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || KebabCaseValidator.IsValid(uniqueName),
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' of inspection must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotTooLong(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || uniqueName.Length <= 140,
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' of inspection can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasValidValue(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || UniqueNameValidator.IsValid(uniqueName),
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotEmpty(string displayName)
            => (!string.IsNullOrWhiteSpace(displayName),
                () => (nameof(displayName), $"Display name of inspection can not be empty"));

        public static (bool, Func<(string, FormattableString)>) DisplayNameIsNotTooLong(string displayName)
            => (string.IsNullOrWhiteSpace(displayName) || displayName.Length <= 140,
                () => (nameof(displayName), $"Display name '{displayName}' of inspection can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) TextIsNotNull(string text)
            => (text != null,
                () => (nameof(text), $"Text of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) TextIsNotTooLong(string text)
            => (text == null || text.Length <= 4000,
                () => (nameof(text), $"Text '{text}' of inspection can not have more than 4000 characters"));

        public static (bool, Func<(string, FormattableString)>) AuditInspectorIsNotNull(string auditInspector)
            => (auditInspector != null,
                () => (nameof(auditInspector), $"Audit inspector '{auditInspector}' of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) AuditInspectorHasKebabCase(string auditInspector)
            => (auditInspector == null || KebabCaseValidator.IsValid(auditInspector),
                () => (nameof(auditInspector), $"Audit inspector '{auditInspector}' of inspection must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) AuditInspectorIsNotTooLong(string auditInspector)
            => (auditInspector == null || auditInspector.Length <= 140,
                () => (nameof(auditInspector), $"Audit inspector '{auditInspector}' of inspection can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) AuditInspectorHasValidValue(string auditInspector)
            => (auditInspector == null || UniqueNameValidator.IsValid(auditInspector),
                () => (nameof(auditInspector), $"Audit inspector '{auditInspector} of inspection' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) AuditAnnotationIsNotNull(string auditAnnotation)
            => (auditAnnotation != null,
                () => (nameof(auditAnnotation), $"Audit annotation of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) AuditAnnotationIsNotTooLong(string auditAnnotation)
            => (auditAnnotation == null || auditAnnotation.Length <= 4000,
                () => (nameof(auditAnnotation), $"Audit annotation '{auditAnnotation}' of inspection can not have more than 4000 characters"));

        public static (bool, Func<(string, FormattableString)>) AuditResultIsNotNull(string auditResult)
            => (auditResult != null,
                () => (nameof(auditResult), $"Audit result of inspection can not be null"));

        public static (bool, Func<(string, FormattableString)>) AuditResultHasValidValue(string auditResult)
            => (auditResult == null || AuditResultValidator.IsValid(auditResult),
                () => (nameof(auditResult), $"Audit result '{auditResult}' of inspection is not a valid value"));

        public static (bool, Func<(string, FormattableString)>) AuditDateIsPositive(int auditDate)
            => (DayNumberValidator.IsValid(auditDate),
                () => (nameof(auditDate), $"Audit date '{auditDate}' of inspection must be positive"));

        public static (bool, Func<(string, FormattableString)>) AuditTimeIsInDayTimeRange(int auditTime)
            => (MillisecondsValidator.IsValid(auditTime),
                () => (nameof(auditTime), $"Audit time '{auditTime}' of inspection must be positive and less than 86400000"));

        public static (bool, Func<(string, FormattableString)>) AssignmentDateIsPositive(int assignmentDate)
            => (DayNumberValidator.IsValid(assignmentDate),
                () => (nameof(assignmentDate), $"Assignment date '{assignmentDate}' of inspection must be positive"));

        public static (bool, Func<(string, FormattableString)>) AssignmentTimeIsInDayTimeRange(int assignmentTime)
            => (MillisecondsValidator.IsValid(assignmentTime),
                () => (nameof(assignmentTime), $"Assignment time '{assignmentTime}' of inspection must be positive and less than 86400000"));
    }
}
