using Super.Paula.Aggregates.Administration;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Management
{
    public static class OrganizationValidator
    {
        public static (Func<bool, bool>, FormattableString) OrganizationHasValue(string organization)
            => (_ => !string.IsNullOrWhiteSpace(organization),
                    $"Organization must have a value");

        public static (Func<bool, bool>, FormattableString) OrganizationHasKebabCase(string organization)
            => (x => x && KebabCaseValidator.IsValid(organization),
                    $"Organization '{organization}' must be in kebab case");

        public static (Func<bool, bool>, FormattableString) OrganizationExists(string organization, IQueryable<Organization> organizations)
            => (x => x && organizations.FirstOrDefault(x => x.UniqueName == organization) != null,
                    $"Organization '{organization}' does not exist");

        public static (Func<bool, bool>, FormattableString) UniqueNameHasValue(Organization organization)
            => (_ => !string.IsNullOrWhiteSpace(organization.UniqueName), 
                    $"Unique name of organization must have a value");

        public static (Func<bool, bool>, FormattableString) UniqueNameHasKebabCase(Organization organization)
            => (x => x && KebabCaseValidator.IsValid(organization.UniqueName), 
                    $"Unique name '{organization.UniqueName}' of organization must be in kebab case");

        public static (Func<bool, bool>, FormattableString) UniqueNameIsUnqiue(Organization organization, IQueryable<Organization> organizations)
            => (x => x && organizations.FirstOrDefault(x => x.UniqueName == organization.UniqueName) == null, 
                    $"Unique name '{organization.UniqueName}' of organization already exists");

        public static (Func<bool, bool>, FormattableString) UniqueNameExists(Organization organization, IQueryable<Organization> organizations)
            => (x => x && organizations.FirstOrDefault(x => x.UniqueName == organization.UniqueName) == null,
                    $"Unique name '{organization.UniqueName}' of organization does not exist");

        public static (Func<bool, bool>, FormattableString) ChiefInspectorIsNotNull(Organization organization)
            => (_ => organization.ChiefInspector != null,
                    $"Chief inspector of organization '{organization.UniqueName}' can not be null");

        public static (Func<bool, bool>, FormattableString) ChiefInspectorHasKebabCase(Organization organization)
            => (x => x && KebabCaseValidator.IsValid(organization.ChiefInspector),
                    $"Chief inspector '{organization.ChiefInspector}' of organization '{organization.UniqueName}' must be in kebab case");

    }
}
