using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionAuditRecordManager : IBusinessObjectInspectionAuditRecordManager
    {
        private readonly IRepository<BusinessObjectInspectionAuditRecord> _repository;

        public BusinessObjectInspectionAuditRecordManager(IRepository<BusinessObjectInspectionAuditRecord> repository)
        {
            _repository = repository;
        }

        public async ValueTask<BusinessObjectInspectionAuditRecord> GetAsync(string businessObject, string inspection, int date, int time)
        {
            EnsureGetable(businessObject, inspection, date, time);
            var entity = await _repository.GetByIdsOrDefaultAsync(date, businessObject, inspection, time);
            if (entity == null)
            {
                throw new ManagementException($"Business object inspection audit record '{businessObject}/{inspection}/{date}/{time}' was not found.");
            }

            return entity;
        }

        public async ValueTask<BusinessObjectInspectionAuditRecord?> GetOrDefaultAsync(string businessObject, string inspection, int date, int time)
        {
            EnsureGetable(businessObject, inspection, date, time);

            return await _repository.GetByIdsOrDefaultAsync(date, businessObject, inspection, time);
        }

        public IQueryable<BusinessObjectInspectionAuditRecord> GetQueryable()
            => _repository.GetQueryable();

        public IAsyncEnumerable<BusinessObjectInspectionAuditRecord> GetAsyncEnumerable(
            CancellationToken cancellationToken = default)

            => _repository.GetAsyncEnumerable(cancellationToken);

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(
            Func<IQueryable<BusinessObjectInspectionAuditRecord>, IQueryable<TResult>> query,
            CancellationToken cancellationToken = default)

            => _repository.GetAsyncEnumerable(query, cancellationToken);

        public IQueryable<BusinessObjectInspectionAuditRecord> GetDateBasedQueryable(int date)
        {
            EnsureGetableDateBased(date);
            return _repository.GetPartitionQueryable(date);
        }

        public IAsyncEnumerable<BusinessObjectInspectionAuditRecord> GetDateBasedAsyncEnumerable(int date)
        {
            EnsureGetableDateBased(date);
            return _repository.GetPartitionAsyncEnumerable(date);
        }

        public IAsyncEnumerable<TResult> GetDateBasedAsyncEnumerable<TResult>(int date, Func<IQueryable<BusinessObjectInspectionAuditRecord>, IQueryable<TResult>> query)
        {
            EnsureGetableDateBased(date);
            return _repository.GetPartitionAsyncEnumerable(query, date);
        }

        public async ValueTask InsertAsync(BusinessObjectInspectionAuditRecord audit)
        {
            EnsureInsertable(audit);

            try
            {
                await _repository.InsertAsync(audit);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert business object inspection audit record '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}'.", exception);
            }
        }

        public async ValueTask UpdateAsync(BusinessObjectInspectionAuditRecord audit)
        {
            EnsureUpdateable(audit);

            try
            {
                await _repository.UpdateAsync(audit);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update business object inspection audit record '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}'.", exception);
            }
        }

        public async ValueTask DeleteAsync(BusinessObjectInspectionAuditRecord audit)
        {
            EnsureDeletable(audit);

            try
            {
                await _repository.DeleteAsync(audit);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete business object inspection audit record '{audit.BusinessObject}/{audit.Inspection}/{audit.AuditDate}/{audit.AuditTime}'.", exception);
            }
        }

        private static void EnsureGetableDateBased(int date)
            => Validator.Ensure($"date for business object inspection audit records",
                BusinessObjectInspectionAuditRecordValidator.AuditDateIsPositive(date));

        private static void EnsureGetable(string businessObject, string inspection, int date, int time)
            => Validator.Ensure($"id '{date}/{businessObject}/{inspection}/{time}' of business object inspection audit record",
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotEmpty(businessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotTooLong(businessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasKebabCase(businessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasValidValue(businessObject),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotEmpty(inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotTooLong(inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasKebabCase(inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasValidValue(inspection),
                BusinessObjectInspectionAuditRecordValidator.AuditDateIsPositive(date),
                BusinessObjectInspectionAuditRecordValidator.AuditTimeIsInDayTimeRange(time));

        private static void EnsureInsertable(BusinessObjectInspectionAuditRecord audit)
            => Validator.Ensure($"business object inspection audit record with id '{audit.AuditDate}/{audit.BusinessObject}/{audit.Inspection}/{audit.AuditTime}'",
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotEmpty(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotTooLong(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasKebabCase(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasValidValue(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotEmpty(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotTooLong(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasKebabCase(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasValidValue(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.AuditDateIsPositive(audit.AuditDate),
                BusinessObjectInspectionAuditRecordValidator.AuditTimeIsInDayTimeRange(audit.AuditTime),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectDisplayNameIsNotEmpty(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectDisplayNameIsNotTooLong(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditRecordValidator.InspectionDisplayNameIsNotEmpty(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditRecordValidator.InspectionDisplayNameIsNotTooLong(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditRecordValidator.InspectorIsNotEmpty(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.InspectorIsNotTooLong(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.InspectorHasKebabCase(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.InspectorHasValidValue(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.ResultIsNotNull(audit.Result),
                BusinessObjectInspectionAuditRecordValidator.ResultHasValidValue(audit.Result),
                BusinessObjectInspectionAuditRecordValidator.AnnotationIsNotNull(audit.Annotation),
                BusinessObjectInspectionAuditRecordValidator.AnnotationIsNotTooLong(audit.Annotation));

        private static void EnsureUpdateable(BusinessObjectInspectionAuditRecord audit)
            => Validator.Ensure($"business object inspection audit record with id '{audit.AuditDate}/{audit.BusinessObject}/{audit.Inspection}/{audit.AuditTime}'",
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotEmpty(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotTooLong(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasKebabCase(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasValidValue(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotEmpty(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotTooLong(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasKebabCase(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasValidValue(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.AuditDateIsPositive(audit.AuditDate),
                BusinessObjectInspectionAuditRecordValidator.AuditTimeIsInDayTimeRange(audit.AuditTime),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectDisplayNameIsNotEmpty(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectDisplayNameIsNotTooLong(audit.BusinessObjectDisplayName),
                BusinessObjectInspectionAuditRecordValidator.InspectionDisplayNameIsNotEmpty(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditRecordValidator.InspectionDisplayNameIsNotTooLong(audit.InspectionDisplayName),
                BusinessObjectInspectionAuditRecordValidator.InspectorIsNotEmpty(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.InspectorIsNotTooLong(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.InspectorHasKebabCase(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.InspectorHasValidValue(audit.Inspector),
                BusinessObjectInspectionAuditRecordValidator.ResultIsNotNull(audit.Result),
                BusinessObjectInspectionAuditRecordValidator.ResultHasValidValue(audit.Result),
                BusinessObjectInspectionAuditRecordValidator.AnnotationIsNotNull(audit.Annotation),
                BusinessObjectInspectionAuditRecordValidator.AnnotationIsNotTooLong(audit.Annotation));

        private static void EnsureDeletable(BusinessObjectInspectionAuditRecord audit)
            => Validator.Ensure($"business object inspection audit record with id '{audit.AuditDate}/{audit.BusinessObject}/{audit.Inspection}/{audit.AuditTime}'",
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotEmpty(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectIsNotTooLong(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasKebabCase(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.BusinessObjectHasValidValue(audit.BusinessObject),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotEmpty(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionIsNotTooLong(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasKebabCase(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.InspectionHasValidValue(audit.Inspection),
                BusinessObjectInspectionAuditRecordValidator.AuditDateIsPositive(audit.AuditDate),
                BusinessObjectInspectionAuditRecordValidator.AuditTimeIsInDayTimeRange(audit.AuditTime));
    }
}