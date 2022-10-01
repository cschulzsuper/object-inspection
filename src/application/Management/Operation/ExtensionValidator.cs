using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public static class ExtensionValidator
{
    public static (bool, Func<(string, FormattableString)>) AggregateTypeIsNotEmpty(string aggregateType)
        => (!string.IsNullOrWhiteSpace(aggregateType),
            () => (nameof(aggregateType), $"Aggregate type can not be empty"));

    public static (bool, Func<(string, FormattableString)>) AggregateTypeHasKebabCase(string aggregateType)
        => (string.IsNullOrWhiteSpace(aggregateType) || KebabCaseValidator.IsValid(aggregateType),
            () => (nameof(aggregateType), $"Aggregate type '{aggregateType}' must have kebab case"));

    public static (bool, Func<(string, FormattableString)>) AggregateTypeIsNotTooLong(string aggregateType)
        => (string.IsNullOrWhiteSpace(aggregateType) || aggregateType.Length <= 140,
            () => (nameof(aggregateType), $"Aggregate type '{aggregateType}' can not have more than 140 characters"));

    public static (bool, Func<(string, FormattableString)>) AggregateTypeHasValidValue(string aggregateType)
        => (string.IsNullOrWhiteSpace(aggregateType) || ExtensionAggregateTypeValidator.IsValid(aggregateType),
            () => (nameof(aggregateType), $"Aggregate type '{aggregateType}' has an invalid value"));

    public static (bool, Func<(string, FormattableString)>) FieldUniqueNamesAreUnique(ICollection<ExtensionField> fields)
        => (!fields.GroupBy(x => x.UniqueName).Any(x => x.Count() > 1),
            () => (nameof(fields), $"Fields with unique name duplicates are not allowed in extension"));

    public static (bool, Func<(string, FormattableString)>) FieldDataNamesAreUnique(ICollection<ExtensionField> fields)
        => (!fields.GroupBy(x => x.DataName).Any(x => x.Count() > 1),
            () => (nameof(fields), $"Fields with data name duplicates are not allowed in extension"));

}