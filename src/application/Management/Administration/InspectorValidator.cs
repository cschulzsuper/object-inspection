using System;
using System.Linq;
using System.Net.Mail;
using Super.Paula.Validation;

namespace Super.Paula.Application.Administration
{
    public static class InspectorValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasValue(string uniqueName)
            => (_ => !string.IsNullOrWhiteSpace(uniqueName),
                    () => $"Unique name of inspector must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasKebabCase(string uniqueName)
            => (_ => KebabCaseValidator.IsValid(uniqueName),
                    () => $"Unique name '{uniqueName}' of inspector must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) MailAddressIsNotNull(Inspector inspector)
            => (_ => inspector.MailAddress != null,
                    () => $"Mail address of inspector '{inspector.UniqueName}' can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) MailAddressIsMailAddress(Inspector inspector)
            => (_ => MailAddress.TryCreate(inspector.MailAddress, out var _),
                    () => $"Mail address of inspector '{inspector.UniqueName}' is not a mail address");

    }
}
