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

        public ValueTask InsertAsync(Inspector inspector)
        {
            EnsureInsertable(inspector);
            return _inspectorRepository.InsertAsync(inspector);
        }

        public ValueTask UpdateAsync(Inspector inspector)
        {
            EnsureUpdateable(inspector);
            return _inspectorRepository.UpdateAsync(inspector);
        }

        public ValueTask DeleteAsync(Inspector inspector)
        {
            EnsureDeleteable(inspector);
            return _inspectorRepository.DeleteAsync(inspector);
        }


        private void EnsureGetable(string inspector)
            => Validator.Ensure(
                InspectorValidator.InspectorHasValue(inspector),
                InspectorValidator.InspectorHasKebabCase(inspector));

        private void EnsureInsertable(Inspector inspector)
            => Validator.Ensure(
                InspectorValidator.UniqueNameHasValue(inspector),
                InspectorValidator.UniqueNameHasKebabCase(inspector),
                InspectorValidator.MailAddressIsNotNull(inspector),
                InspectorValidator.MailAddressIsMailAddress(inspector));

        private void EnsureUpdateable(Inspector inspector)
            => Validator.Ensure(
                InspectorValidator.UniqueNameHasValue(inspector),
                InspectorValidator.UniqueNameHasKebabCase(inspector),
                InspectorValidator.MailAddressIsNotNull(inspector),
                InspectorValidator.MailAddressIsMailAddress(inspector));

        private void EnsureDeleteable(Inspector inspector)
            => Validator.Ensure(
                InspectorValidator.UniqueNameHasValue(inspector),
                InspectorValidator.UniqueNameHasKebabCase(inspector));
    }
}