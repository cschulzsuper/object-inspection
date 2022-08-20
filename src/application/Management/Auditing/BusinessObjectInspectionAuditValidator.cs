using Super.Paula.Shared.Validation;
using System;

namespace Super.Paula.Application.Auditing;

public static class BusinessObjectInspectionAuditValidator
{
    public static (bool, Func<(string, FormattableString)>) InspectorIsNotNull(string inspector)
        => (inspector != null,
            () => (nameof(inspector), $"Inspector '{inspector}' of audit can not be null"));

    public static (bool, Func<(string, FormattableString)>) InspectorHasKebabCase(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || KebabCaseValidator.IsValid(inspector),
            () => (nameof(inspector), $"Inspector '{inspector}' of audit must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) InspectorIsNotTooLong(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || inspector.Length <= 140,
            () => (nameof(inspector), $"Inspector '{inspector}' of audit can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) InspectorHasValidValue(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || UniqueNameValidator.IsValid(inspector),
            () => (nameof(inspector), $"Inspector '{inspector}' of audit has an invalid value"));

    public static (bool, Func<(string, FormattableString)>) AnnotationIsNotNull(string annotation)
        => (annotation != null,
            () => (nameof(annotation), $"Annotation of audit can not be null"));

    public static (bool, Func<(string, FormattableString)>) AnnotationIsNotTooLong(string annotation)
        => (annotation == null || annotation.Length <= 4000,
            () => (nameof(annotation), $"Annotation '{annotation}' of audit can not have more than 4000 characters"));

    public static (bool, Func<(string, FormattableString)>) ResultIsNotNull(string result)
        => (result != null,
            () => (nameof(result), $"Result of audit can not be null"));

    public static (bool, Func<(string, FormattableString)>) ResultHasValidValue(string result)
        => (result == null || AuditResultValidator.IsValid(result),
            () => (nameof(result), $"Result '{result}' of audit is not a valid value"));

    public static (bool, Func<(string, FormattableString)>) AuditDateIsPositive(int auditDate)
        => (DayNumberValidator.IsValid(auditDate),
            () => (nameof(auditDate), $"Audit date '{auditDate}' of inspection must be positive"));

    public static (bool, Func<(string, FormattableString)>) AuditTimeIsInDayTimeRange(int auditTime)
        => (MillisecondsValidator.IsValid(auditTime),
            () => (nameof(auditTime), $"Audit time '{auditTime}' of inspection must be positive and less than 86400000"));
}