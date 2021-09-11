using Super.Paula.Aggregates.Inventory;
using Super.Paula.Data;

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
            => _businessObjectRepository.GetByIdAsync(businessObject);

        public IQueryable<BusinessObject> GetQueryable()
            => _businessObjectRepository.GetPartitionQueryable();

        public IAsyncEnumerable<BusinessObject> GetAsyncEnumerable()
            => _businessObjectRepository.GetPartitionAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObject>, IQueryable<TResult>> query)
            => _businessObjectRepository.GetPartitionAsyncEnumerable(query);

        public ValueTask InsertAsync(BusinessObject businessObject)
            => _businessObjectRepository.InsertAsync(businessObject);

        public ValueTask UpdateAsync(BusinessObject businessObject)
            => _businessObjectRepository.UpdateAsync(businessObject);

        public ValueTask DeleteAsync(BusinessObject businessObject)
            => _businessObjectRepository.DeleteAsync(businessObject);
    }
}