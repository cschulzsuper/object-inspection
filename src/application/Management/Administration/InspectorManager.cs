using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class InspectorManager : IInspectorManager
    {
        private readonly IRepository<Inspector> _inspectorRepository;

        public InspectorManager(IRepository<Inspector> inspectorRepository)
        {
            _inspectorRepository = inspectorRepository;
        }

        public async ValueTask<Inspector> GetAsync(string inspector)
        {
            EnsureGetable(inspector);

            var entity = await _inspectorRepository.GetByIdsOrDefaultAsync(inspector);
            if (entity == null)
            {
                throw new ManagementException($"Inspector '{inspector}' was not found.");
            }

            return entity;
        }

        public IQueryable<Inspector> GetQueryable()
            => _inspectorRepository.GetPartitionQueryable();

        public IQueryable<Inspector> GetQueryableWhereBusinessObject(string businessObject)
            => _inspectorRepository.GetPartitionQueryable(
                $"SELECT * FROM c WHERE ARRAY_CONTAINS(c.BusinessObjects, {{\"UniqueName\": {businessObject}}}, true)");

        public IAsyncEnumerable<Inspector> GetAsyncEnumerable()
            => _inspectorRepository.GetPartitionAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspector>, IQueryable<TResult>> query)
            => _inspectorRepository.GetPartitionAsyncEnumerable(query);

        public async ValueTask InsertAsync(Inspector inspector)
        {
            EnsureInsertable(inspector);

            try
            {
                await _inspectorRepository.InsertAsync(inspector);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert inspector '{inspector.UniqueName}'.", exception);
            }
        }

        public async ValueTask UpdateAsync(Inspector inspector)
        {
            EnsureUpdateable(inspector);

            try
            {
                await _inspectorRepository.UpdateAsync(inspector);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update inspector '{inspector.UniqueName}'.", exception);
            }
        }

        public async ValueTask DeleteAsync(Inspector inspector)
        {
            EnsureDeletable(inspector);

            try
            {
                await _inspectorRepository.DeleteAsync(inspector);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete inspector '{inspector.UniqueName}'.", exception);
            }
        }

        private static void EnsureGetable(string inspector)
            => Validator.Ensure($"unique name '{inspector}' of inspector",
                InspectorValidator.UniqueNameIsNotEmpty(inspector),
                InspectorValidator.UniqueNameHasKebabCase(inspector),
                InspectorValidator.UniqueNameIsNotTooLong(inspector),
                InspectorValidator.UniqueNameHasValidValue(inspector));

        private static void EnsureInsertable(Inspector inspector)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return InspectorValidator.UniqueNameIsNotEmpty(inspector.UniqueName);
                yield return InspectorValidator.UniqueNameHasKebabCase(inspector.UniqueName);
                yield return InspectorValidator.UniqueNameIsNotTooLong(inspector.UniqueName);
                yield return InspectorValidator.UniqueNameHasValidValue(inspector.UniqueName);
                yield return InspectorValidator.IdentityIsNotNull(inspector.Identity);
                yield return InspectorValidator.IdentityHasKebabCase(inspector.Identity);
                yield return InspectorValidator.IdentityIsNotTooLong(inspector.Identity);
                yield return InspectorValidator.IdentityHasValidValue(inspector.Identity);
                yield return InspectorValidator.OrganizationIsNotEmpty(inspector.Organization);
                yield return InspectorValidator.OrganizationHasKebabCase(inspector.Organization);
                yield return InspectorValidator.OrganizationIsNotTooLong(inspector.Organization);
                yield return InspectorValidator.OrganizationHasValidValue(inspector.Organization);
                yield return InspectorValidator.OrganizationDisplayNameIsNotEmpty(inspector.OrganizationDisplayName);
                yield return InspectorValidator.OrganizationDisplayNameIsNotTooLong(inspector.OrganizationDisplayName);

                yield return InspectorBusinessObjectValidator.BusinessObjectsUnique(inspector.BusinessObjects);

                foreach (var businessObjects in inspector.BusinessObjects)
                {
                    yield return InspectorBusinessObjectValidator.UniqueNameIsNotEmpty(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.UniqueNameHasKebabCase(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.UniqueNameIsNotTooLong(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.UniqueNameHasValidValue(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.DisplayNameIsNotEmpty(businessObjects.DisplayName);
                    yield return InspectorBusinessObjectValidator.DisplayNameIsNotTooLong(businessObjects.DisplayName);

                    yield return InspectorBusinessObjectValidator.AuditSchedulePlannedAuditDateIsPositive(businessObjects.AuditSchedulePlannedAuditDate);
                    yield return InspectorBusinessObjectValidator.AuditSchedulePlannedAuditTimeIsInDayTimeRange(businessObjects.AuditSchedulePlannedAuditTime);
                }
            }

            Validator.Ensure($"inspector with unique name '{inspector.UniqueName}'", Ensurences());
        }

        private static void EnsureUpdateable(Inspector inspector)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return InspectorValidator.UniqueNameIsNotEmpty(inspector.UniqueName);
                yield return InspectorValidator.UniqueNameHasKebabCase(inspector.UniqueName);
                yield return InspectorValidator.UniqueNameIsNotTooLong(inspector.UniqueName);
                yield return InspectorValidator.UniqueNameHasValidValue(inspector.UniqueName);
                yield return InspectorValidator.IdentityIsNotNull(inspector.Identity);
                yield return InspectorValidator.IdentityHasKebabCase(inspector.Identity);
                yield return InspectorValidator.IdentityIsNotTooLong(inspector.Identity);
                yield return InspectorValidator.IdentityHasValidValue(inspector.Identity);
                yield return InspectorValidator.OrganizationIsNotEmpty(inspector.Organization);
                yield return InspectorValidator.OrganizationHasKebabCase(inspector.Organization);
                yield return InspectorValidator.OrganizationIsNotTooLong(inspector.Organization);
                yield return InspectorValidator.OrganizationHasValidValue(inspector.Organization);
                yield return InspectorValidator.OrganizationDisplayNameIsNotEmpty(inspector.OrganizationDisplayName);
                yield return InspectorValidator.OrganizationDisplayNameIsNotTooLong(inspector.OrganizationDisplayName);

                yield return InspectorBusinessObjectValidator.BusinessObjectsUnique(inspector.BusinessObjects);

                foreach (var businessObjects in inspector.BusinessObjects)
                {
                    yield return InspectorBusinessObjectValidator.UniqueNameIsNotEmpty(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.UniqueNameHasKebabCase(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.UniqueNameIsNotTooLong(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.UniqueNameHasValidValue(businessObjects.UniqueName);
                    yield return InspectorBusinessObjectValidator.DisplayNameIsNotEmpty(businessObjects.DisplayName);
                    yield return InspectorBusinessObjectValidator.DisplayNameIsNotTooLong(businessObjects.DisplayName);

                    yield return InspectorBusinessObjectValidator.AuditSchedulePlannedAuditDateIsPositive(businessObjects.AuditSchedulePlannedAuditDate);
                    yield return InspectorBusinessObjectValidator.AuditSchedulePlannedAuditTimeIsInDayTimeRange(businessObjects.AuditSchedulePlannedAuditTime);
                }
            }

            Validator.Ensure($"inspector with unique name '{inspector.UniqueName}'", Ensurences());
        }

        private static void EnsureDeletable(Inspector inspector)
            => Validator.Ensure($"inspector with unique name '{inspector.UniqueName}'",
                InspectorValidator.UniqueNameIsNotEmpty(inspector.UniqueName),
                InspectorValidator.UniqueNameHasKebabCase(inspector.UniqueName),
                InspectorValidator.UniqueNameIsNotTooLong(inspector.UniqueName),
                InspectorValidator.UniqueNameHasValidValue(inspector.UniqueName));
    }
}