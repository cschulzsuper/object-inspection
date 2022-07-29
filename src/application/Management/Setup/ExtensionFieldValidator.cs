using Super.Paula.Validation;
using System;

namespace Super.Paula.Application.Setup
{
    public static class ExtensionFieldValidator
    {
        public static (bool, Func<(string, FormattableString)>) TypeIsNotEmpty(string type)
            => (!string.IsNullOrWhiteSpace(type),
                () => (nameof(type), $"Type can not be empty"));

        public static (bool, Func<(string, FormattableString)>) TypeHasLowerCase(string type)
            => (string.IsNullOrWhiteSpace(type) || LowerCaseValidator.IsValid(type),
                () => (nameof(type), $"Type '{type}' must have lower case"));

        public static (bool, Func<(string, FormattableString)>) TypeIsNotTooLong(string type)
            => (string.IsNullOrWhiteSpace(type) || type.Length <= 140,
                () => (nameof(type), $"Type '{type}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) TypeHasValidValue(string type)
            => (string.IsNullOrWhiteSpace(type) || ExtensionFieldTypeValidator.IsValid(type),
                () => (nameof(type), $"Type '{type}' has an invalid value"));

        public static (bool, Func<(string, FormattableString)>) NameIsNotEmpty(string name)
            => (!string.IsNullOrWhiteSpace(name),
                () => (nameof(name), $"Name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) NameHasCamelCase(string name)
            => (string.IsNullOrWhiteSpace(name) || CamelCaseValidator.IsValid(name),
                () => (nameof(name), $"Name '{name}' must have camel case"));

        public static (bool, Func<(string, FormattableString)>) NameIsNotTooLong(string name)
            => (string.IsNullOrWhiteSpace(name) || name.Length <= 140,
                () => (nameof(name), $"Name '{name}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) NameHasValidValue(string name)
            => (string.IsNullOrWhiteSpace(name) || ExtensionFieldTypeValidator.IsValid(name),
                () => (nameof(name), $"Name '{name}' has an invalid value"));
    }
}
