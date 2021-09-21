using System;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Auditing
{
    public static class BusinessObjectInspectionAuditValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) InspectionHasValue(string inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection),
                    () => $"Inspection must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionHasKebabCase(string inspection)
            => (_ => KebabCaseValidator.IsValid(inspection),
                    () => $"Inspection '{inspection}' must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectHasValue(string businessObject)
            => (_ => !string.IsNullOrWhiteSpace(businessObject),
                    () => $"Business object must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectHasKebabCase(string businessObject)
            => (_ => KebabCaseValidator.IsValid(businessObject),
                    () => $"Business object '{businessObject}' must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) AuditDateIsPositive(int date)
            => (_ => date >= 0,
                    () => $"Audit date '{date}' must be positive");

        public static (Func<bool, bool>, Func<FormattableString>) AuditTimeIsInDayTimeRange(int time)
            => (_ => time >= 0 && time < 86400000,
                    () => $"Audit time '{time}' must be positive and less than 86400000");

        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectHasValue(BusinessObjectInspectionAudit audit)
            => (_ => !string.IsNullOrWhiteSpace(audit.BusinessObject),
                    () => $"Business object of business object inspection audit must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectHasKebabCase(BusinessObjectInspectionAudit audit)
            => (_ => KebabCaseValidator.IsValid(audit.BusinessObject),
                    () => $"Business object '{audit.BusinessObject}' of business object inspection audit must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionHasValue(BusinessObjectInspectionAudit audit)
            => (_ => !string.IsNullOrWhiteSpace(audit.Inspection),
                    () => $"Inspection of business object inspection audit must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionHasKebabCase(BusinessObjectInspectionAudit audit)
            => (_ => KebabCaseValidator.IsValid(audit.Inspection),
                    () => $"Inspection '{audit.Inspection}' of business object inspection audit must be in kebab case");

       public static (Func<bool, bool>, Func<FormattableString>) InspectorHasValue(BusinessObjectInspectionAudit audit)
            => (_ => !string.IsNullOrWhiteSpace(audit.Inspector),
                    () => $"Inspector of business object inspection audit must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectorHasKebabCase(BusinessObjectInspectionAudit audit)
            => (_ => KebabCaseValidator.IsValid(audit.Inspector),
                    () => $"Inspector '{audit.Inspection}' of business object inspection audit must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionDisplayNameHasValue(BusinessObjectInspectionAudit audit)
            => (_ => !string.IsNullOrWhiteSpace(audit.InspectionDisplayName),
                    () => $"Inspection display name of business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}' must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) BusinessObjectDisplayNameHasValue(BusinessObjectInspectionAudit audit)
            => (_ => !string.IsNullOrWhiteSpace(audit.BusinessObjectDisplayName),
                    () => $"Business object display name of business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}' must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) ResultHasValidValue(BusinessObjectInspectionAudit audit)
            => (_ => (new[] { string.Empty, "satisfying", "insufficient", "failed" }).Contains(audit.Result),
                    () => $"Result '{audit.Result}' of business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}' is not a valid value");

        public static (Func<bool, bool>, Func<FormattableString>) AuditDateIsPositive(BusinessObjectInspectionAudit audit)
            => (_ => audit.AuditDate >= 0,
                    () => $"Audit date '{audit.AuditDate}' of business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}' must be positive");

        public static (Func<bool, bool>, Func<FormattableString>) AuditTimeIsInDayTimeRange(BusinessObjectInspectionAudit audit)
            => (_ => audit.AuditTime >= 0 && audit.AuditTime < 86400000,
                    () => $"Audit time '{audit.AuditTime}' of business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}' must be positive and less than 86400000");

        public static (Func<bool, bool>, Func<FormattableString>) AnnotationIsNotNull(BusinessObjectInspectionAudit audit)
            => (_ => audit.Annotation != null,
                    () => $"Annotation of business object inspection '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}' can not be null");
    }
}
