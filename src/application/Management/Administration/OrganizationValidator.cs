using Super.Paula.Validation;

namespace Super.Paula.Application.Administration
{
    public static class OrganizationValidator
    {
        public static (Func<bool, bool>, Func<FormattableString>) OrganizationHasValue(string organization)
            => (_ => !string.IsNullOrWhiteSpace(organization),
                    () => $"Organization must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) OrganizationHasKebabCase(string organization)
            => (_ => KebabCaseValidator.IsValid(organization),
                    () => $"Organization '{organization}' must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) OrganizationExists(string organization, IQueryable<Organization> organizations)
            => (x => !x || organizations.FirstOrDefault(x => x.UniqueName == organization) != null,
                    () => $"Organization '{organization}' does not exist");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasValue(Organization organization)
            => (_ => !string.IsNullOrWhiteSpace(organization.UniqueName),
                    () => $"Unique name of organization must have a value");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameHasKebabCase(Organization organization)
            => (_ => KebabCaseValidator.IsValid(organization.UniqueName),
                    () => $"Unique name '{organization.UniqueName}' of organization must be in kebab case");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameIsUnqiue(Organization organization, IQueryable<Organization> organizations)
            => (x => !x || organizations.FirstOrDefault(x => x.UniqueName == organization.UniqueName) == null,
                    () => $"Unique name '{organization.UniqueName}' of organization already exists");

        public static (Func<bool, bool>, Func<FormattableString>) UniqueNameExists(Organization organization, IQueryable<Organization> organizations)
            => (x => !x || organizations.FirstOrDefault(x => x.UniqueName == organization.UniqueName) != null,
                    () => $"Unique name '{organization.UniqueName}' of organization does not exist");

        public static (Func<bool, bool>, Func<FormattableString>) ChiefInspectorIsNotNull(Organization organization)
            => (_ => organization.ChiefInspector != null,
                    () => $"Chief inspector of organization '{organization.UniqueName}' can not be null");

        public static (Func<bool, bool>, Func<FormattableString>) ChiefInspectorHasKebabCase(Organization organization)
            => (_ => KebabCaseValidator.IsValid(organization.ChiefInspector),
                    () => $"Chief inspector '{organization.ChiefInspector}' of organization '{organization.UniqueName}' must be in kebab case");

    }
}
