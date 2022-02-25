using Super.Paula.Validation;
using System;

namespace Super.Paula.Application.Auditing
{
    public static class BusinessObjectInspectionAuditRecordValidator
    {
        public static (bool, Func<(string, FormattableString)>) InspectionIsNotEmpty(string inspection)
            => (!string.IsNullOrWhiteSpace(inspection),
                () => (nameof(inspection), $"Inspection can not be empty"));

        public static (bool, Func<(string, FormattableString)>) InspectionHasKebabCase(string inspection)
            => (string.IsNullOrWhiteSpace(inspection) || KebabCaseValidator.IsValid(inspection),
                () => (nameof(inspection), $"Inspection '{inspection}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) InspectionIsNotTooLong(string inspection)
            => (string.IsNullOrWhiteSpace(inspection) || inspection.Length <= 140,
                () => (nameof(inspection), $"Inspection '{inspection}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectionHasValidValue(string inspection)
            => (string.IsNullOrWhiteSpace(inspection) || UniqueNameValidator.IsValid(inspection),
                () => (nameof(inspection), $"Inspection '{inspection}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) BusinessObjectIsNotEmpty(string businessObject)
            => (!string.IsNullOrWhiteSpace(businessObject),
                () => (nameof(businessObject), $"Business object can not be empty"));

        public static (bool, Func<(string, FormattableString)>) BusinessObjectHasKebabCase(string businessObject)
            => (string.IsNullOrWhiteSpace(businessObject) || KebabCaseValidator.IsValid(businessObject),
                () => (nameof(businessObject), $"Business object '{businessObject}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) BusinessObjectIsNotTooLong(string businessObject)
            => (string.IsNullOrWhiteSpace(businessObject) || businessObject.Length <= 140,
                () => (nameof(businessObject), $"Business object '{businessObject}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) BusinessObjectHasValidValue(string businessObject)
            => (string.IsNullOrWhiteSpace(businessObject) || UniqueNameValidator.IsValid(businessObject),
                () => (nameof(businessObject), $"Business object '{businessObject}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) AuditDateIsPositive(int auditDate)
            => (DayNumberValidator.IsValid(auditDate),
                () => (nameof(auditDate), $"Audit date '{auditDate}' must be positive"));

        public static (bool, Func<(string, FormattableString)>) AuditTimeIsInDayTimeRange(int auditTime)
            => (MillisecondsValidator.IsValid(auditTime),
                () => (nameof(auditTime), $"Audit time '{auditTime}' must be positive and less than 86400000"));

        public static (bool, Func<(string, FormattableString)>) InspectorIsNotEmpty(string inspector)
            => (!string.IsNullOrWhiteSpace(inspector),
                () => (nameof(inspector), $"Inspector can not be null"));

        public static (bool, Func<(string, FormattableString)>) InspectorHasKebabCase(string inspector)
            => (string.IsNullOrWhiteSpace(inspector) || KebabCaseValidator.IsValid(inspector),
                () => (nameof(inspector), $"Inspector '{inspector}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) InspectorIsNotTooLong(string inspector)
            => (string.IsNullOrWhiteSpace(inspector) || inspector.Length <= 140,
                () => (nameof(inspector), $"Inspector '{inspector}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) InspectorHasValidValue(string inspector)
            => (string.IsNullOrWhiteSpace(inspector) || UniqueNameValidator.IsValid(inspector),
                () => (nameof(inspector), $"Inspector '{inspector}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) InspectionDisplayNameIsNotEmpty(string inspectionDisplayName)
            => (!string.IsNullOrWhiteSpace(inspectionDisplayName),
                () => (nameof(inspectionDisplayName), $"Inspection display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) InspectionDisplayNameIsNotTooLong(string inspectionDisplayName)
            => (string.IsNullOrWhiteSpace(inspectionDisplayName) || inspectionDisplayName.Length <= 140,
                () => (nameof(inspectionDisplayName), $"Inspection display name '{inspectionDisplayName}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) BusinessObjectDisplayNameIsNotEmpty(string bussinesObjectDisplayName)
            => (!string.IsNullOrWhiteSpace(bussinesObjectDisplayName),
                () => (nameof(bussinesObjectDisplayName), $"Business object display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) BusinessObjectDisplayNameIsNotTooLong(string bussinesObjectDisplayName)
            => (string.IsNullOrWhiteSpace(bussinesObjectDisplayName) || bussinesObjectDisplayName.Length <= 140,
                () => (nameof(bussinesObjectDisplayName), $"Business object display name '{bussinesObjectDisplayName}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) ResultIsNotNull(string result)
            => (result != null,
                () => (nameof(result), $"Result can not be null"));

        public static (bool, Func<(string, FormattableString)>) ResultHasValidValue(string result)
            => (result == null || AuditResultValidator.IsValid(result),
                () => (nameof(result), $"Result '{result}' is not a valid value"));

        public static (bool, Func<(string, FormattableString)>) AnnotationIsNotNull(string annotation)
            => (annotation != null,
                () => (nameof(annotation), $"Annotation can not be null"));

        public static (bool, Func<(string, FormattableString)>) AnnotationIsNotTooLong(string annotation)
            => (annotation == null || annotation.Length <= 4000,
                () => (nameof(annotation), $"Annotation '{annotation}' can not have more than 4000 characters"));
    }
}
