using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public static class BusinessObjectInspectionValidator
{
    public static (bool, Func<(string, FormattableString)>) BusinessObjectIsNotEmpty(string businessObject)
       => (!string.IsNullOrWhiteSpace(businessObject),
           () => (nameof(businessObject), $"Business object of business object inspection can not be empty"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectHasKebabCase(string businessObject)
        => (string.IsNullOrWhiteSpace(businessObject) || KebabCaseValidator.IsValid(businessObject),
            () => (nameof(businessObject), $"Business object '{businessObject}' of business object inspection must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectIsNotTooLong(string businessObject)
        => (string.IsNullOrWhiteSpace(businessObject) || businessObject.Length <= 140,
            () => (nameof(businessObject), $"Business object '{businessObject}' of business object inspection can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectHasValidValue(string businessObject)
        => (string.IsNullOrWhiteSpace(businessObject) || UniqueNameValidator.IsValid(businessObject),
            () => (nameof(businessObject), $"Business object '{businessObject}' has an invalid value"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectDisplayNameIsNotEmpty(string businessObjectDisplayName)
        => (!string.IsNullOrWhiteSpace(businessObjectDisplayName),
            () => (nameof(businessObjectDisplayName), $"Business object display name of business object inspection can not be empty"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectDisplayNameIsNotTooLong(string businessObjectDisplayName)
        => (string.IsNullOrWhiteSpace(businessObjectDisplayName) || businessObjectDisplayName.Length <= 140,
            () => (nameof(businessObjectDisplayName), $"Business object display name '{businessObjectDisplayName}' of business object inspection can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) InspectionIsNotEmpty(string inspection)
        => (!string.IsNullOrWhiteSpace(inspection),
            () => (nameof(inspection), $"Inspection of business object inspection can not be empty"));

    public static (bool, Func<(string, FormattableString)>) InspectionHasKebabCase(string inspection)
        => (string.IsNullOrWhiteSpace(inspection) || KebabCaseValidator.IsValid(inspection),
            () => (nameof(inspection), $"Inspection '{inspection}' of business object inspection must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) InspectionIsNotTooLong(string inspection)
        => (string.IsNullOrWhiteSpace(inspection) || inspection.Length <= 140,
            () => (nameof(inspection), $"Inspection '{inspection}' of business object inspection can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) InspectionHasValidValue(string inspection)
        => (string.IsNullOrWhiteSpace(inspection) || UniqueNameValidator.IsValid(inspection),
            () => (nameof(inspection), $"Inspection '{inspection}' has an invalid value"));

    public static (bool, Func<(string, FormattableString)>) InspectionDisplayNameIsNotEmpty(string inspectionDisplayName)
        => (!string.IsNullOrWhiteSpace(inspectionDisplayName),
            () => (nameof(inspectionDisplayName), $"Inspection display name of business object inspection can not be empty"));

    public static (bool, Func<(string, FormattableString)>) InspectionDisplayNameIsNotTooLong(string inspectionDisplayName)
        => (string.IsNullOrWhiteSpace(inspectionDisplayName) || inspectionDisplayName.Length <= 140,
            () => (nameof(inspectionDisplayName), $"Inspection display name '{inspectionDisplayName}' of business object inspection can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) InspectionTextIsNotNull(string inspectionText)
        => (inspectionText != null,
            () => (nameof(inspectionText), $"Inspection text of business object inspection can not be null"));

    public static (bool, Func<(string, FormattableString)>) InspectionTextIsNotTooLong(string text)
        => (text == null || text.Length <= 4000,
            () => (nameof(text), $"Text '{text}' of inspection can not have more than 4000 characters"));

    public static (bool, Func<(string, FormattableString)>) AssignmentDateIsPositive(int assignmentDate)
        => (DayNumberValidator.IsValid(assignmentDate),
            () => (nameof(assignmentDate), $"Assignment date '{assignmentDate}' of inspection must be positive"));

    public static (bool, Func<(string, FormattableString)>) AssignmentTimeIsInDayTimeRange(int assignmentTime)
        => (MillisecondsValidator.IsValid(assignmentTime),
            () => (nameof(assignmentTime), $"Assignment time '{assignmentTime}' of inspection must be positive and less than 86400000"));
}