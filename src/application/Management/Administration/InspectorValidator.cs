using System;
using System.Net.Mail;
using Super.Paula.Validation;

namespace Super.Paula.Application.Administration
{
    public static class InspectorValidator
    {
        public static (bool, Func<(string, FormattableString)>) UniqueNameHasValue(string uniqueName)
            => (!string.IsNullOrWhiteSpace(uniqueName),
                () => (nameof(Inspector.UniqueName), $"Unique name must have a value"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasKebabCase(string uniqueName)
            => (KebabCaseValidator.IsValid(uniqueName),
                () => (nameof(Inspector.UniqueName), $"Unique name '{uniqueName}' must be in kebab case"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotTooLong(string inspector)
            => (inspector == null || inspector.Length <= 140,
                () => (nameof(Inspector.UniqueName), $"Unique name can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsNotNull(string mailAddress)
            => (mailAddress != null,
                () => (nameof(Inspector.MailAddress), $"Mail address can not be null"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsMailAddress(string mailAddress)
            => (MailAddress.TryCreate(mailAddress, out var _),
                () => (nameof(Inspector.MailAddress), $"Mail address '{mailAddress}' is not a mail address"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsNotTooLong(string mailAddress)
            => (mailAddress == null || mailAddress.Length <= 140,
                () => (nameof(Inspector.MailAddress), $"Mail address can not have more than 140 characters"));
    }
}
