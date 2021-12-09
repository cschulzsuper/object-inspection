using System;
using System.Net.Mail;
using Super.Paula.Validation;

namespace Super.Paula.Application.Administration
{
    public static class IdentityValidator
    {
        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotEmpty(string uniqueName)
            => (!string.IsNullOrWhiteSpace(uniqueName),
                () => (nameof(uniqueName), $"Unique name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasKebabCase(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || KebabCaseValidator.IsValid(uniqueName),
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' must have kebab case"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotTooLong(string uniqueName)
            => (string.IsNullOrWhiteSpace(uniqueName) || uniqueName.Length <= 140,
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsNotNull(string mailAddress)
            => (mailAddress != null,
                () => (nameof(mailAddress), $"Mail address can not be null"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsMailAddress(string mailAddress)
            => (mailAddress == null || MailAddress.TryCreate(mailAddress, out _),
                () => (nameof(mailAddress), $"Mail address '{mailAddress}' is not a mail address"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsNotTooLong(string mailAddress)
            => (mailAddress == null || mailAddress.Length <= 140,
                () => (nameof(mailAddress), $"Mail address '{mailAddress}' can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) SecretHasValue(string secret)
            => (!string.IsNullOrWhiteSpace(secret),
                () => (nameof(secret), $"Secret can not be empty"));

        public static (bool, Func<(string, FormattableString)>) SecretIsNotTooLong(string secret)
            => (string.IsNullOrWhiteSpace(secret) || secret.Length <= 140,
                () => (nameof(secret), $"Secret '{secret}' can not have more than 140 characters"));
    }
}
