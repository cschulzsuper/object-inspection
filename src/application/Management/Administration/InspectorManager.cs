using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class  InspectorManager : IInspectorManager
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
                throw new ManagementException($"Inspector '{inspector}' was not found");
            }

            return entity;
        }

        public IQueryable<Inspector> GetQueryable()
            => _inspectorRepository.GetPartitionQueryable();

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
                throw new ManagementException($"Could not insert inspector '{inspector.UniqueName}'", exception);
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
                throw new ManagementException($"Could not update inspector '{inspector.UniqueName}'", exception);
            }
        }

        public async ValueTask DeleteAsync(Inspector inspector)
        {
            EnsureDeleteable(inspector);

            try
            {
                await _inspectorRepository.DeleteAsync(inspector);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete inspector '{inspector.UniqueName}'", exception);
            }
        }

        private static void EnsureGetable(string inspector)
            => Validator.Ensure($"unique name '{inspector}' of inspector",
                InspectorValidator.UniqueNameIsNotEmpty(inspector),
                InspectorValidator.UniqueNameHasKebabCase(inspector),
                InspectorValidator.UniqueNameIsNotTooLong(inspector));

        private static void EnsureInsertable(Inspector inspector)
            => Validator.Ensure($"inspector with unique name '{inspector.UniqueName}'",
                InspectorValidator.UniqueNameIsNotEmpty(inspector.UniqueName),
                InspectorValidator.UniqueNameHasKebabCase(inspector.UniqueName),
                InspectorValidator.UniqueNameIsNotTooLong(inspector.UniqueName),
                InspectorValidator.MailAddressIsNotNull(inspector.MailAddress),
                InspectorValidator.MailAddressIsMailAddress(inspector.MailAddress),
                InspectorValidator.MailAddressIsNotTooLong(inspector.MailAddress),
                InspectorValidator.OrganizationIsNotEmpty(inspector.Organization),
                InspectorValidator.OrganizationHasKebabCase(inspector.Organization),
                InspectorValidator.OrganizationIsNotTooLong(inspector.Organization),
                InspectorValidator.OrganizationDisplayNameIsNotEmpty(inspector.OrganizationDisplayName),
                InspectorValidator.OrganizationDisplayNameIsNotTooLong(inspector.OrganizationDisplayName),
                InspectorValidator.ProofHasValue(inspector.Proof),
                InspectorValidator.ProofIsNotTooLong(inspector.Proof),
                InspectorValidator.SecretHasValue(inspector.Secret),
                InspectorValidator.SecretIsNotTooLong(inspector.Secret));

        private static void EnsureUpdateable(Inspector inspector)
            => Validator.Ensure($"inspector with unique name '{inspector.UniqueName}'",
                InspectorValidator.UniqueNameIsNotEmpty(inspector.UniqueName),
                InspectorValidator.UniqueNameHasKebabCase(inspector.UniqueName),
                InspectorValidator.UniqueNameIsNotTooLong(inspector.UniqueName),
                InspectorValidator.MailAddressIsNotNull(inspector.MailAddress),
                InspectorValidator.MailAddressIsMailAddress(inspector.MailAddress),
                InspectorValidator.MailAddressIsNotTooLong(inspector.MailAddress),
                InspectorValidator.OrganizationIsNotEmpty(inspector.Organization),
                InspectorValidator.OrganizationHasKebabCase(inspector.Organization),
                InspectorValidator.OrganizationIsNotTooLong(inspector.Organization),
                InspectorValidator.OrganizationDisplayNameIsNotEmpty(inspector.OrganizationDisplayName),
                InspectorValidator.OrganizationDisplayNameIsNotTooLong(inspector.OrganizationDisplayName),
                InspectorValidator.ProofHasValue(inspector.Proof),
                InspectorValidator.ProofIsNotTooLong(inspector.Proof),
                InspectorValidator.SecretHasValue(inspector.Secret),
                InspectorValidator.SecretIsNotTooLong(inspector.Secret));

        private static void EnsureDeleteable(Inspector inspector)
            => Validator.Ensure($"inspector with unique name '{inspector.UniqueName}'",
                InspectorValidator.UniqueNameIsNotEmpty(inspector.UniqueName),
                InspectorValidator.UniqueNameHasKebabCase(inspector.UniqueName),
                InspectorValidator.UniqueNameIsNotTooLong(inspector.UniqueName));
    }
}