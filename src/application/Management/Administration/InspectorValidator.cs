using System;
using System.Net.Mail;
using Super.Paula.Validation;

namespace Super.Paula.Application.Administration
{
    public static class InspectorValidator
    {
        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotEmpty(string uniqueName)
            => (!string.IsNullOrWhiteSpace(uniqueName),
                () => (nameof(uniqueName), $"Unique name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameHasKebabCase(string uniqueName)
            => (KebabCaseValidator.IsValid(uniqueName),
                () => (nameof(uniqueName), $"Unique name '{uniqueName}' must be in kebab case"));

        public static (bool, Func<(string, FormattableString)>) UniqueNameIsNotTooLong(string uniqueName)
            => (uniqueName == null || uniqueName.Length <= 140,
                () => (nameof(uniqueName), $"Unique name can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsNotNull(string mailAddress)
            => (mailAddress != null,
                () => (nameof(mailAddress), $"Mail address can not be null"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsMailAddress(string mailAddress)
            => (MailAddress.TryCreate(mailAddress, out var _),
                () => (nameof(mailAddress), $"Mail address '{mailAddress}' is not a mail address"));

        public static (bool, Func<(string, FormattableString)>) MailAddressIsNotTooLong(string mailAddress)
            => (mailAddress == null || mailAddress.Length <= 140,
                () => (nameof(mailAddress), $"Mail address can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) OrganizationIsNotEmpty(string organization)
            => (!string.IsNullOrWhiteSpace(organization),
                () => (nameof(organization), $"Organization can not be empty"));

        public static (bool, Func<(string, FormattableString)>) OrganizationHasKebabCase(string organization)
            => (KebabCaseValidator.IsValid(organization),
                () => (nameof(organization), $"Organization '{organization}' must be in kebab case"));

        public static (bool, Func<(string, FormattableString)>) OrganizationIsNotTooLong(string organization)
            => (organization == null || organization.Length <= 140,
                () => (nameof(organization), $"Organization can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) OrganizationDisplayNameHasValue(string organizationDisplayName)
            => (!string.IsNullOrWhiteSpace(organizationDisplayName),
                () => (nameof(organizationDisplayName), $"Organization display name can not be empty"));

        public static (bool, Func<(string, FormattableString)>) OrganizationDisplayNameIsNotTooLong(string organizationDisplayName)
            => (organizationDisplayName == null || organizationDisplayName.Length <= 140,
                () => (nameof(organizationDisplayName), $"Organization display name can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) ProofHasValue(string proof)
            => (proof != null,
                () => (nameof(proof), $"Proof can not be null"));

        public static (bool, Func<(string, FormattableString)>) ProofIsNotTooLong(string proof)
            => (proof == null || proof.Length <= 140,
                () => (nameof(proof), $"Proof can not have more than 140 characters"));

        public static (bool, Func<(string, FormattableString)>) SecretHasValue(string secret)
            => (!string.IsNullOrWhiteSpace(secret),
                () => (nameof(secret), $"Secret can not be empty"));

        public static (bool, Func<(string, FormattableString)>) SecretIsNotTooLong(string secret)
            => (secret == null || secret.Length <= 140,
                () => (nameof(secret), $"Secret can not have more than 140 characters"));
    }
}
