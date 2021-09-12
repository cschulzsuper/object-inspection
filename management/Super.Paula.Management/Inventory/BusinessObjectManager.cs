using Super.Paula.Aggregates.Inventory;
using Super.Paula.Data;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Management.Inventory
{
    public class  BusinessObjectManager : IBusinessObjectManager
    {
        private readonly IRepository<BusinessObject> _businessObjectRepository;

        public BusinessObjectManager(IRepository<BusinessObject> businessObjectRepository)
        {
            _businessObjectRepository = businessObjectRepository;
        }

        public ValueTask<BusinessObject> GetAsync(string businessObject)
        {
            EnsureGetable(businessObject);
            return _businessObjectRepository.GetByIdAsync(businessObject);
        }

        public IQueryable<BusinessObject> GetQueryable()
            => _businessObjectRepository.GetPartitionQueryable();

        public IAsyncEnumerable<BusinessObject> GetAsyncEnumerable()
            => _businessObjectRepository.GetPartitionAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObject>, IQueryable<TResult>> query)
            => _businessObjectRepository.GetPartitionAsyncEnumerable(query);

        public ValueTask InsertAsync(BusinessObject businessObject)
        {
            EnsureInsertable(businessObject);
            return _businessObjectRepository.InsertAsync(businessObject);
        }

        public ValueTask UpdateAsync(BusinessObject businessObject)
        {
            EnsureUpdateable(businessObject);
            return _businessObjectRepository.UpdateAsync(businessObject);
        }

        public ValueTask DeleteAsync(BusinessObject businessObject)
        {
            EnsureDeleteable(businessObject);
            return _businessObjectRepository.DeleteAsync(businessObject);
        }

        private void EnsureGetable(string businessObject)
            => Validator.Ensure(
                BusinessObjectValidator.BusinessObjectHasValue(businessObject),
                BusinessObjectValidator.BusinessObjectHasKebabCase(businessObject),
                BusinessObjectValidator.BusinessObjectExists(businessObject, GetQueryable()));

        private void EnsureInsertable(BusinessObject businessObject)
            => Validator.Ensure(
                BusinessObjectValidator.UniqueNameHasValue(businessObject),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject),
                BusinessObjectValidator.UniqueNameIsUnqiue(businessObject, GetQueryable()),
                BusinessObjectValidator.DisplayNameHasValue(businessObject),
                BusinessObjectValidator.InspectorIsNotNull(businessObject),
                BusinessObjectValidator.InspectorHasKebabCase(businessObject));

        private void EnsureUpdateable(BusinessObject businessObject)
            => Validator.Ensure(
                BusinessObjectValidator.UniqueNameHasValue(businessObject),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject),
                BusinessObjectValidator.UniqueNameExists(businessObject, GetQueryable()),
                BusinessObjectValidator.DisplayNameHasValue(businessObject),
                BusinessObjectValidator.InspectorIsNotNull(businessObject),
                BusinessObjectValidator.InspectorHasKebabCase(businessObject));

        private void EnsureDeleteable(BusinessObject businessObject)
            => Validator.Ensure(
                BusinessObjectValidator.UniqueNameHasValue(businessObject),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject),
                BusinessObjectValidator.UniqueNameExists(businessObject, GetQueryable()));
    }
}