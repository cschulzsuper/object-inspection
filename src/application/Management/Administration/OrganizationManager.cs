using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class OrganizationManager : IOrganizationManager
    {
        private readonly IRepository<Organization> _organizationRepository;

        public OrganizationManager(IRepository<Organization> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async ValueTask<Organization> GetAsync(string organization)
        {
            EnsureGetable(organization);

            var entity = await _organizationRepository.GetByIdsOrDefaultAsync(organization);
            if (entity == null)
            {
                throw new ManagementException($"Organization '{organization}' was not found.");
            }

            return entity;
        }

        public async ValueTask InsertAsync(Organization organization)
        {
            EnsureInsertable(organization);

            try
            {
                await _organizationRepository.InsertAsync(organization);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert organization '{organization.UniqueName}'.", exception);
            }
        }

        public async ValueTask UpdateAsync(Organization organization)
        {
            EnsureUpdateable(organization);

            try
            {
                await _organizationRepository.UpdateAsync(organization);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update organization '{organization.UniqueName}'.", exception);
            }
        }

        public async ValueTask DeleteAsync(Organization organization)
        {
            EnsureDeletable(organization);

            try
            {
                await _organizationRepository.DeleteAsync(organization);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete organization '{organization.UniqueName}'.", exception);
            }
        }

        public IQueryable<Organization> GetQueryable()
            => _organizationRepository.GetQueryable();

        public IAsyncEnumerable<Organization> GetAsyncEnumerable()
            => _organizationRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Organization>, IQueryable<TResult>> query)
            => _organizationRepository.GetAsyncEnumerable(query);

        private static void EnsureGetable(string organization)
            => Validator.Ensure($"unique name '{organization}' of organization",
                OrganizationValidator.UniqueNameIsNotEmpty(organization),
                OrganizationValidator.UniqueNameHasKebabCase(organization),
                OrganizationValidator.UniqueNameIsNotTooLong(organization),
                OrganizationValidator.UniqueNameHasValidValue(organization));

        private static void EnsureInsertable(Organization organization)
            => Validator.Ensure($"organization with unique name '{organization.UniqueName}'",
                OrganizationValidator.UniqueNameIsNotEmpty(organization.UniqueName),
                OrganizationValidator.UniqueNameHasKebabCase(organization.UniqueName),
                OrganizationValidator.UniqueNameIsNotTooLong(organization.UniqueName),
                OrganizationValidator.UniqueNameHasValidValue(organization.UniqueName),
                OrganizationValidator.DisplayNameIsNotEmpty(organization.DisplayName),
                OrganizationValidator.DisplayNameIsNotTooLong(organization.DisplayName),
                OrganizationValidator.ChiefInspectorIsNotEmpty(organization.ChiefInspector),
                OrganizationValidator.ChiefInspectorHasKebabCase(organization.ChiefInspector),
                OrganizationValidator.ChiefInspectorIsNotTooLong(organization.ChiefInspector),
                OrganizationValidator.ChiefInspectorHasValidValue(organization.ChiefInspector));

        private static void EnsureUpdateable(Organization organization)
            => Validator.Ensure($"organization with unique name '{organization.UniqueName}'",
                OrganizationValidator.UniqueNameIsNotEmpty(organization.UniqueName),
                OrganizationValidator.UniqueNameHasKebabCase(organization.UniqueName),
                OrganizationValidator.UniqueNameIsNotTooLong(organization.UniqueName),
                OrganizationValidator.UniqueNameHasValidValue(organization.UniqueName),
                OrganizationValidator.DisplayNameIsNotEmpty(organization.DisplayName),
                OrganizationValidator.DisplayNameIsNotTooLong(organization.DisplayName),
                OrganizationValidator.ChiefInspectorIsNotEmpty(organization.ChiefInspector),
                OrganizationValidator.ChiefInspectorHasKebabCase(organization.ChiefInspector),
                OrganizationValidator.ChiefInspectorIsNotTooLong(organization.ChiefInspector),
                OrganizationValidator.ChiefInspectorHasValidValue(organization.ChiefInspector));

        private static void EnsureDeletable(Organization organization)
            => Validator.Ensure($"organization with unique name '{organization.UniqueName}'",
                OrganizationValidator.UniqueNameIsNotEmpty(organization.UniqueName),
                OrganizationValidator.UniqueNameHasKebabCase(organization.UniqueName),
                OrganizationValidator.UniqueNameIsNotTooLong(organization.UniqueName),
                OrganizationValidator.UniqueNameHasValidValue(organization.UniqueName));
    }
}