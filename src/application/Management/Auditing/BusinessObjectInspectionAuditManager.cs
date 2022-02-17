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

        public async ValueTask<BusinessObjectInspectionAudit> GetAsync(string businessObject, string inspection, int date, int time)
        {
            EnsureGetable(businessObject, inspection, date, time);
            var entity = await _businessObjectInspectionAuditRepository.GetByIdsOrDefaultAsync(date, businessObject, inspection, time);
            if (entity == null)
            {
                throw new ManagementException($"Business object inspection audit '{businessObject}/{inspection}/{date}/{time}' was not found");
            }

            return entity;
        }

        public async ValueTask<BusinessObjectInspectionAudit?> GetOrDefaultAsync(string businessObject, string inspection, int date, int time)
        {
            EnsureGetable(businessObject, inspection, date, time);

            return await _businessObjectInspectionAuditRepository.GetByIdsOrDefaultAsync(date, businessObject, inspection, time);
        }

        public IQueryable<BusinessObjectInspectionAudit> GetQueryable()
            => _businessObjectInspectionAuditRepository.GetQueryable();

        public IAsyncEnumerable<BusinessObjectInspectionAudit> GetAsyncEnumerable()
            => _businessObjectInspectionAuditRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspectionAudit>, IQueryable<TResult>> query)
            => _businessObjectInspectionAuditRepository.GetAsyncEnumerable(query);

        public IQueryable<BusinessObjectInspectionAudit> GetDateBasedQueryable(int date)
        {
            EnsureGetableDateBased(date);
            return _businessObjectInspectionAuditRepository.GetPartitionQueryable(date);
        }

        public IAsyncEnumerable<BusinessObjectInspectionAudit> GetDateBasedAsyncEnumerable(int date)
        {
            EnsureGetableDateBased(date);
            return _businessObjectInspectionAuditRepository.GetPartitionAsyncEnumerable(date);
        }

        public IAsyncEnumerable<TResult> GetDateBasedAsyncEnumerable<TResult>(int date, Func<IQueryable<BusinessObjectInspectionAudit>, IQueryable<TResult>> query)
        {
            EnsureGetableDateBased(date);
            return _businessObjectInspectionAuditRepository.GetPartitionAsyncEnumerable(query, date);
        }

        public async ValueTask InsertAsync(BusinessObjectInspectionAudit audit)
        {
            EnsureInsertable(audit);

            try
            {
                await _businessObjectInspectionAuditRepository.InsertAsync(audit);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}'", exception);
            }
        }

        public async ValueTask UpdateAsync(BusinessObjectInspectionAudit audit)
        {
            EnsureUpdateable(audit);

            try
            {
                await _businessObjectInspectionAuditRepository.UpdateAsync(audit);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}'", exception);
            }
        }

        public async ValueTask DeleteAsync(BusinessObjectInspectionAudit audit)
        {
            EnsureDeletable(audit);

            try
            {
                await _businessObjectInspectionAuditRepository.DeleteAsync(audit);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete business object inspection audit '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}'", exception);
            }
        }

        private static void EnsureGetableDateBased(int date)
            => Validator.Ensure($"date for business object inspection audits",
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(date));

        private static void EnsureGetable(string businessObject, string inspection, int date, int time)
            => Validator.Ensure($"id '{date}/{businessObject}/{inspection}/{time}' of notification",
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotEmpty(businessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotTooLong(businessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(businessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValidValue(businessObject),
                BusinessObjectInspectionAuditValidator.InspectionIsNotEmpty(inspection),
                BusinessObjectInspectionAuditValidator.InspectionIsNotTooLong(inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasValidValue(inspection),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(date),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(time));

        private static void EnsureInsertable(BusinessObjectInspectionAudit audit)
            => Validator.Ensure($"business object inspection audit with id '{audit.AuditDate}/{audit.BusinessObject}/{audit.Inspection}/{audit.AuditTime}'",
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotEmpty(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotTooLong(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValidValue(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.InspectionIsNotEmpty(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionIsNotTooLong(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasValidValue(audit.Inspection),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(audit.AuditDate),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(audit.AuditTime),
                BusinessObjectInspectionAuditValidator.BusinessObjectDisplayNameIsNotEmpty(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditValidator.BusinessObjectDisplayNameIsNotTooLong(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditValidator.InspectionDisplayNameIsNotEmpty(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditValidator.InspectionDisplayNameIsNotTooLong(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditValidator.InspectorIsNotEmpty(audit.Inspector),
                BusinessObjectInspectionAuditValidator.InspectorIsNotTooLong(audit.Inspector),
                BusinessObjectInspectionAuditValidator.InspectorHasKebabCase(audit.Inspector),
                BusinessObjectInspectionAuditValidator.InspectorHasValidValue(audit.Inspector),
                BusinessObjectInspectionAuditValidator.ResultIsNotNull(audit.Result),
                BusinessObjectInspectionAuditValidator.ResultHasValidValue(audit.Result),
                BusinessObjectInspectionAuditValidator.AnnotationIsNotNull(audit.Annotation),
                BusinessObjectInspectionAuditValidator.AnnotationIsNotTooLong(audit.Annotation));

        private static void EnsureUpdateable(BusinessObjectInspectionAudit audit)
            => Validator.Ensure($"business object inspection audit with id '{audit.AuditDate}/{audit.BusinessObject}/{audit.Inspection}/{audit.AuditTime}'",
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotEmpty(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotTooLong(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValidValue(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.InspectionIsNotEmpty(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionIsNotTooLong(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasValidValue(audit.Inspection),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(audit.AuditDate),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(audit.AuditTime),
                BusinessObjectInspectionAuditValidator.BusinessObjectDisplayNameIsNotEmpty(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditValidator.BusinessObjectDisplayNameIsNotTooLong(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditValidator.InspectionDisplayNameIsNotEmpty(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditValidator.InspectionDisplayNameIsNotTooLong(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditValidator.InspectorIsNotEmpty(audit.Inspector),
                BusinessObjectInspectionAuditValidator.InspectorIsNotTooLong(audit.Inspector),
                BusinessObjectInspectionAuditValidator.InspectorHasKebabCase(audit.Inspector),
                BusinessObjectInspectionAuditValidator.InspectorHasValidValue(audit.Inspector),
                BusinessObjectInspectionAuditValidator.ResultIsNotNull(audit.Result),
                BusinessObjectInspectionAuditValidator.ResultHasValidValue(audit.Result),
                BusinessObjectInspectionAuditValidator.AnnotationIsNotNull(audit.Annotation),
                BusinessObjectInspectionAuditValidator.AnnotationIsNotTooLong(audit.Annotation));

        private static void EnsureDeletable(BusinessObjectInspectionAudit audit)
            => Validator.Ensure($"business object inspection audit with id '{audit.AuditDate}/{audit.BusinessObject}/{audit.Inspection}/{audit.AuditTime}'",
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotEmpty(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectIsNotTooLong(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasKebabCase(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.BusinessObjectHasValidValue(audit.BusinessObject),
                BusinessObjectInspectionAuditValidator.InspectionIsNotEmpty(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionIsNotTooLong(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasKebabCase(audit.Inspection),
                BusinessObjectInspectionAuditValidator.InspectionHasValidValue(audit.Inspection),
                BusinessObjectInspectionAuditValidator.AuditDateIsPositive(audit.AuditDate),
                BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(audit.AuditTime));
    }
}