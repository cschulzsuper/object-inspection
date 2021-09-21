using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Guidlines
{
    public class  InspectionManager : IInspectionManager
    {
        private readonly IRepository<Inspection> _inspectionRepository;

        public InspectionManager(IRepository<Inspection> inspectionRepository)
        {
            _inspectionRepository = inspectionRepository;
        }

        public async ValueTask<Inspection> GetAsync(string inspection)
        {
            EnsureGetable(inspection);

            var entity = await _inspectionRepository.GetByIdsOrDefaultAsync(inspection);
            if (entity == null)
            {
                throw new ManagementException($"Inspection '{inspection}' was not found");
            }

            return entity;
        }

        public IQueryable<Inspection> GetQueryable()
            => _inspectionRepository.GetPartitionQueryable();

        public IAsyncEnumerable<Inspection> GetAsyncEnumerable()
            => _inspectionRepository.GetPartitionAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspection>, IQueryable<TResult>> query)
            => _inspectionRepository.GetPartitionAsyncEnumerable(query);

        public async ValueTask InsertAsync(Inspection inspection)
        {
            EnsureInsertable(inspection);

            try
            {
                await _inspectionRepository.InsertAsync(inspection);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert inspection '{inspection.UniqueName}'", exception);
            }
        }

        public async ValueTask UpdateAsync(Inspection inspection)
        {
            EnsureUpdateable(inspection);

            try
            {
                await _inspectionRepository.UpdateAsync(inspection);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update inspection '{inspection.UniqueName}'", exception);
            }
        }

        public async ValueTask DeleteAsync(Inspection inspection)
        {
            EnsureDeleteable(inspection);

            try
            {
                await _inspectionRepository.DeleteAsync(inspection);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete inspection '{inspection.UniqueName}'", exception);
            }
        }

        private void EnsureGetable(string inspection)
            => Validator.Ensure(
                InspectionValidator.UniqueNameHasValue(inspection),
                InspectionValidator.UniqueNameHasKebabCase(inspection));

        private void EnsureInsertable(Inspection inspection)
            => Validator.Ensure(
                InspectionValidator.UniqueNameHasValue(inspection.UniqueName),
                InspectionValidator.UniqueNameHasKebabCase(inspection.UniqueName),
                InspectionValidator.DisplayNameHasValue(inspection));

        private void EnsureUpdateable(Inspection inspection)
            => Validator.Ensure(
                InspectionValidator.UniqueNameHasValue(inspection.UniqueName),
                InspectionValidator.UniqueNameHasKebabCase(inspection.UniqueName),
                InspectionValidator.DisplayNameHasValue(inspection));

        private void EnsureDeleteable(Inspection inspection)
            => Validator.Ensure(
                InspectionValidator.UniqueNameHasValue(inspection.UniqueName),
                InspectionValidator.UniqueNameHasKebabCase(inspection.UniqueName));

    }
}