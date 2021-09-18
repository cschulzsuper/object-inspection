using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        {
            EnsureGetable(businessObject, inspection, date, time);
            return _businessObjectInspectionAuditRepository.GetByIdsAsync(date, businessObject, inspection, time);
        }

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

        public ValueTask InsertAsync(BusinessObjectInspectionAudit audit)
        {
            EnsureInsertable(audit);
            return _businessObjectInspectionAuditRepository.InsertAsync(audit);
        }

        public ValueTask UpdateAsync(BusinessObjectInspectionAudit audit)
        {
            EnsureUpdateable(audit);
            return _businessObjectInspectionAuditRepository.UpdateAsync(audit);
        }

        public ValueTask DeleteAsync(BusinessObjectInspectionAudit audit)
        {
            EnsureDeleteable(audit);
            return _businessObjectInspectionAuditRepository.DeleteAsync(audit);
        }

        private void EnsureGetable(string businessObject, string inspection, int date, int time)
            => Validator.Ensure(
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValue(businessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(businessObject),
                BusinessObjectInspectionAuditValidator.InspectionHasValue(businessObject),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(businessObject),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(date),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(time),
                BusinessObjectInspectionAuditValidator.BusinessObjectInspectionAuditExists(businessObject, inspection, date, time, GetDateBasedQueryable(date)));

        private void EnsureInsertable(BusinessObjectInspectionAudit audit)
            => Validator.Ensure(
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValue(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.InspectionHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(audit),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectDisplayNameHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectionDisplayNameHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectorHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectorHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.ResultHasValidValue(audit),
                BusinessObjectInspectionAuditValidator.AnnotationIsNotNull(audit),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(audit),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectInspectionAuditIsUnqiue(audit, GetDateBasedQueryable(audit.AuditDate)));

        private void EnsureUpdateable(BusinessObjectInspectionAudit audit)
            => Validator.Ensure(
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValue(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.InspectionHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(audit),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectDisplayNameHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectionDisplayNameHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectorHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectorHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.ResultHasValidValue(audit),
                BusinessObjectInspectionAuditValidator.AnnotationIsNotNull(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectInspectionAuditExists(audit, GetDateBasedQueryable(audit.AuditDate)));

        private void EnsureDeleteable(BusinessObjectInspectionAudit audit)
            => Validator.Ensure(
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValue(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.InspectionHasValue(audit),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(audit),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(audit),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(audit),
                BusinessObjectInspectionAuditValidator.BusinessObjectInspectionAuditExists(audit, GetDateBasedQueryable(audit.AuditDate)));
    }
}