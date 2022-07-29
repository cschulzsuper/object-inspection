using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Application.Setup
{
    public static class ExtensionValidator
    {
        public static (bool, Func<(string, FormattableString)>) TypeIsNotEmpty(string type)
            => (!string.IsNullOrWhiteSpace(type),
                () => (nameof(type), $"Type can not be empty"));

        public static (bool, Func<(string, FormattableString)>) TypeHasKebabCase(string type)
            => (string.IsNullOrWhiteSpace(type) || KebabCaseValidator.IsValid(type),
                () => (nameof(type), $"Type '{type}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) TypeIsNotTooLong(string type)
            => (string.IsNullOrWhiteSpace(type) || type.Length <= 140,
                () => (nameof(type), $"Type '{type}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) TypeHasValidValue(string type)
            => (string.IsNullOrWhiteSpace(type) || ExtensionTypeValidator.IsValid(type),
                () => (nameof(type), $"Type '{type}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) FieldsUnique(ICollection<ExtensionField> fields)
            => (!fields.GroupBy(x => x.Name).Any(x => x.Count() > 1),
                () => (nameof(fields), $"Field duplicates are not allowed in extension"));
    }
}
