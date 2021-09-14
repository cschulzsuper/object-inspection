using Super.Paula.Data;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionAuditManager : IBusinessObjectInspectionAuditManager
    {
        private readonly IRepository<BusinessObjectInspectionAudit> _businessObjectInspectionAuditRepository;

        public BusinessObjectInspectionAuditManager(IRepository<BusinessObjectInspectionAudit> businessObjectInspectionAuditRepository)
        {
            _businessObjectInspectionAuditRepository = businessObjectInspectionAuditRepository;
        }

        public ValueTask<BusinessObjectInspectionAudit> GetAsync(string businessObject, string inspection, int date, int time)
            => _businessObjectInspectionAuditRepository.GetByIdsAsync(date, businessObject, inspection, time);

        public IQueryable<BusinessObjectInspectionAudit> GetQueryable()
            => _businessObjectInspectionAuditRepository.GetQueryable();

        public IAsyncEnumerable<BusinessObjectInspectionAudit> GetAsyncEnumerable()
            => _businessObjectInspectionAuditRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspectionAudit>, IQueryable<TResult>> query)
            => _businessObjectInspectionAuditRepository.GetAsyncEnumerable(query);

        public IQueryable<BusinessObjectInspectionAudit> GetDateBasedQueryable(int date)
            => _businessObjectInspectionAuditRepository.GetPartitionQueryable(date);

        public IAsyncEnumerable<BusinessObjectInspectionAudit> GetDateBasedAsyncEnumerable(int date)
            => _businessObjectInspectionAuditRepository.GetPartitionAsyncEnumerable(date);

        public IAsyncEnumerable<TResult> GetDateBasedAsyncEnumerable<TResult>(int date, Func<IQueryable<BusinessObjectInspectionAudit>, IQueryable<TResult>> query)
            => _businessObjectInspectionAuditRepository.GetPartitionAsyncEnumerable(query, date);

        public ValueTask InsertAsync(BusinessObjectInspectionAudit businessObjectInspectionAudit)
            => _businessObjectInspectionAuditRepository.InsertAsync(businessObjectInspectionAudit);

        public ValueTask UpdateAsync(BusinessObjectInspectionAudit businessObjectInspectionAudit)
            => _businessObjectInspectionAuditRepository.UpdateAsync(businessObjectInspectionAudit);

        public ValueTask DeleteAsync(BusinessObjectInspectionAudit businessObjectInspectionAudit)
            => _businessObjectInspectionAuditRepository.DeleteAsync(businessObjectInspectionAudit);
    }
}