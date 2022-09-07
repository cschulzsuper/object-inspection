using Super.Paula.Shared.Validation;
using System;

namespace Super.Paula.Application.Auditing;

public static class BusinessObjectInspectorValidator
{
    public static (bool, Func<(string, FormattableString)>) BusinessObjectIsNotEmpty(string businessObject)
       => (!string.IsNullOrWhiteSpace(businessObject),
           () => (nameof(businessObject), $"Business object of business object inspector can not be empty"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectHasKebabCase(string businessObject)
        => (string.IsNullOrWhiteSpace(businessObject) || KebabCaseValidator.IsValid(businessObject),
            () => (nameof(businessObject), $"Business object '{businessObject}' of business object inspector must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectIsNotTooLong(string businessObject)
        => (string.IsNullOrWhiteSpace(businessObject) || businessObject.Length <= 140,
            () => (nameof(businessObject), $"Business object '{businessObject}' of business object inspector can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectHasValidValue(string businessObject)
        => (string.IsNullOrWhiteSpace(businessObject) || UniqueNameValidator.IsValid(businessObject),
            () => (nameof(businessObject), $"Business object '{businessObject}' has an invalid value"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectDisplayNameIsNotEmpty(string businessObjectDisplayName)
        => (!string.IsNullOrWhiteSpace(businessObjectDisplayName),
            () => (nameof(businessObjectDisplayName), $"Business object display name of business object inspector can not be empty"));

    public static (bool, Func<(string, FormattableString)>) BusinessObjectDisplayNameIsNotTooLong(string businessObjectDisplayName)
        => (string.IsNullOrWhiteSpace(businessObjectDisplayName) || businessObjectDisplayName.Length <= 140,
            () => (nameof(businessObjectDisplayName), $"Business object display name '{businessObjectDisplayName}' of business object inspector can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) InspectorIsNotEmpty(string inspector)
        => (!string.IsNullOrWhiteSpace(inspector),
            () => (nameof(inspector), $"Inspector of business object inspector can not be empty"));

    public static (bool, Func<(string, FormattableString)>) InspectorHasKebabCase(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || KebabCaseValidator.IsValid(inspector),
            () => (nameof(inspector), $"Inspector '{inspector}' of business object inspector must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) InspectorIsNotTooLong(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || inspector.Length <= 140,
            () => (nameof(inspector), $"Inspector '{inspector}' of business object inspector can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) InspectorHasValidValue(string inspector)
        => (string.IsNullOrWhiteSpace(inspector) || UniqueNameValidator.IsValid(inspector),
            () => (nameof(inspector), $"Inspector '{inspector}' has an invalid value"));

}