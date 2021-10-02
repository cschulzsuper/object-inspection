using System;
using System.Linq;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory
{
    public static class BusinessObjectValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasValue(string uniqueName)
            => (_ => !string.IsNullOrWhiteSpace(uniqueName),
                    () => $"Unique name of business object must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasKebabCase(string uniqueName)
            => (_ => KebabCaseValidator.IsValid(uniqueName),
                    () => $"Unique name '{uniqueName}' of business object must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) DisplayNameIsNotEmpty(BusinessObject businessObject)
            => (_ => !string.IsNullOrWhiteSpace(businessObject.DisplayName),
                    () => $"Display name of business object '{businessObject.UniqueName}' must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectorIsNotNull(BusinessObject businessObject)
            => (_ => businessObject.Inspector != null,
                    () => $"Inspector '{businessObject.UniqueName}' of business object can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) InspectorHasKebabCase(BusinessObject businessObject)
            => (_ => KebabCaseValidator.IsValid(businessObject.Inspector),
                    () => $"Inspector '{businessObject.Inspector}' of business object '{businessObject.UniqueName}' must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionUniqueNameHasValue(BusinessObject.EmbeddedInspection inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection.UniqueName),
                    () => $"Unique name of business object inspection must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionUniqueNameHasKebabCase(BusinessObject.EmbeddedInspection inspection)
            => (_ => KebabCaseValidator.IsValid(inspection.UniqueName),
                    () => $"Unique name '{inspection.UniqueName}' of business object inspection must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionTextIsNotNull(BusinessObject.EmbeddedInspection inspection)
            => (_ => inspection.Text != null,
                    () => $"Text of business object inspection '{inspection.UniqueName}' can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionDisplayNameIsNotEmpty(BusinessObject.EmbeddedInspection inspection)
            => (_ => !string.IsNullOrWhiteSpace(inspection.DisplayName),
                    () => $"Display name of business object inspection '{inspection.UniqueName}' must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionAuditInspectorIsNotNull(BusinessObject.EmbeddedInspection inspection)
            => (_ => inspection.AuditInspector != null,
                    () => $"Audit inspector of business object inspection '{inspection.UniqueName}' can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionAuditInspectorHasKebabCase(BusinessObject.EmbeddedInspection inspection)
            => (_ => KebabCaseValidator.IsValid(inspection.AuditInspector),
                    () => $"Audit inspector '{inspection.AuditInspector}' of business object inspection '{inspection.UniqueName}' must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionAuditAnnotationIsNotNull(BusinessObject.EmbeddedInspection inspection)
            => (_ => inspection.AuditAnnotation != null,
                    () => $"Audit annotation of business object inspection '{inspection.UniqueName}' can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionAuditResultHasValidValue(BusinessObject.EmbeddedInspection inspection)
            => (_ => (new[] { string.Empty, "satisfying", "insufficient", "failed" }).Contains(inspection.AuditResult),
                    () => $"Audit result '{inspection.AuditResult}' of business object inspection '{inspection.UniqueName}' is not a valid value");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionAuditDateIsPositive(BusinessObject.EmbeddedInspection inspection)
            => (_ => inspection.AuditDate >= 0,
                    () => $"Audit date '{inspection.AuditDate}' of business object inspection '{inspection.UniqueName}' must be positive");

        public static (Func<bool, bool>, Func<FormattableString>) InspectionAuditTimeIsInDayTimeRange(BusinessObject.EmbeddedInspection inspection)
            => (_ => inspection.AuditTime >= 0 && inspection.AuditTime < 86400000,
                    () => $"Audit time '{inspection.AuditTime}' of business object inspection '{inspection.UniqueName}' must be positive and less than 86400000");
    }
}
