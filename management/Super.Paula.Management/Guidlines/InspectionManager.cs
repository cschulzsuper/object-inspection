﻿using Super.Paula.Validation;

namespace Super.Paula.Guidlines
{
    public class  InspectionManager : IInspectionManager
    {
        private readonly IRepository<Inspection> _inspectionRepository;

        public InspectionManager(IRepository<Inspection> inspectionRepository)
        {
            _inspectionRepository = inspectionRepository;
        }

        public ValueTask<Inspection> GetAsync(string inspection)
        {
            EnsureGetable(inspection);
            return _inspectionRepository.GetByIdAsync(inspection);
        }

        public IQueryable<Inspection> GetQueryable()
            => _inspectionRepository.GetPartitionQueryable();

        public IAsyncEnumerable<Inspection> GetAsyncEnumerable()
            => _inspectionRepository.GetPartitionAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspection>, IQueryable<TResult>> query)
            => _inspectionRepository.GetPartitionAsyncEnumerable(query);

        public ValueTask InsertAsync(Inspection inspection)
        {
            EnsureInsertable(inspection);
            return _inspectionRepository.InsertAsync(inspection);
        }

        public ValueTask UpdateAsync(Inspection inspection)
        {
            EnsureUpdateable(inspection);
            return _inspectionRepository.UpdateAsync(inspection);
        }

        public ValueTask DeleteAsync(Inspection inspection)
        {
            EnsureDeleteable(inspection);
            return _inspectionRepository.DeleteAsync(inspection);
        }

        private void EnsureGetable(string inspection)
            => Validator.Ensure(
                InspectionValidator.InspectionHasValue(inspection),
                InspectionValidator.InspectionHasKebabCase(inspection),
                InspectionValidator.InspectionExists(inspection, GetQueryable()));

        private void EnsureInsertable(Inspection inspection)
            => Validator.Ensure(
                InspectionValidator.UniqueNameHasValue(inspection),
                InspectionValidator.UniqueNameHasKebabCase(inspection),
                InspectionValidator.UniqueNameIsUnqiue(inspection, GetQueryable()),
                InspectionValidator.DisplayNameHasValue(inspection));

        private void EnsureUpdateable(Inspection inspection)
            => Validator.Ensure(
                InspectionValidator.UniqueNameHasValue(inspection),
                InspectionValidator.UniqueNameHasKebabCase(inspection),
                InspectionValidator.UniqueNameExists(inspection, GetQueryable()),
                InspectionValidator.DisplayNameHasValue(inspection));

        private void EnsureDeleteable(Inspection inspection)
            => Validator.Ensure(
                InspectionValidator.UniqueNameHasValue(inspection),
                InspectionValidator.UniqueNameHasKebabCase(inspection),
                InspectionValidator.UniqueNameExists(inspection, GetQueryable()));

    }
}