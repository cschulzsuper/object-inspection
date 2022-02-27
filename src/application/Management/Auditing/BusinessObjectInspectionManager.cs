using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionManager : IBusinessObjectInspectionManager
    {
        private readonly IRepository<BusinessObjectInspection> _repository;

        public BusinessObjectInspectionManager(IRepository<BusinessObjectInspection> repository)
        {
            _repository = repository;
        }

        public async ValueTask<BusinessObjectInspection> GetAsync(string businessObject, string inspection)
        {
            EnsureGetable(businessObject, inspection);

            var entity = await _repository.GetByIdsOrDefaultAsync(businessObject, inspection);
            if (entity == null)
            {
                throw new ManagementException($"Business object inspection '{businessObject}/{inspection}' was not found.");
            }

            return entity;
        }

        public IQueryable<BusinessObjectInspection> GetQueryable()
            => _repository.GetQueryable();

        public IAsyncEnumerable<BusinessObjectInspection> GetAsyncEnumerable()
            => _repository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspection>, IQueryable<TResult>> query)
            => _repository.GetAsyncEnumerable(query);

        public async ValueTask InsertAsync(BusinessObjectInspection inspection)
        {
            EnsureInsertable(inspection);

            try
            {
                await _repository.InsertAsync(inspection);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert business object inspection '{inspection.BusinessObject}/{inspection.Inspection}'.", exception);
            }
        }

        public async ValueTask UpdateAsync(BusinessObjectInspection inspection)
        {
            EnsureUpdateable(inspection);

            try
            {
                await _repository.UpdateAsync(inspection);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update business object inspection '{inspection.BusinessObject}/{inspection.Inspection}'.", exception);
            }
        }

        public async ValueTask DeleteAsync(BusinessObjectInspection inspection)
        {
            EnsureDeletable(inspection);

            try
            {
                await _repository.DeleteAsync(inspection);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete business object inspection '{inspection.BusinessObject}/{inspection.Inspection}'.", exception);
            }
        }

        private static void EnsureGetable(string businessObject, string inspection)
            => Validator.Ensure($"id '{businessObject}/{inspection}' of business object inspection",
                BusinessObjectInspectionValidator.BusinessObjectIsNotEmpty(businessObject),
                BusinessObjectInspectionValidator.BusinessObjectHasKebabCase(businessObject),
                BusinessObjectInspectionValidator.BusinessObjectIsNotTooLong(businessObject),
                BusinessObjectInspectionValidator.BusinessObjectHasValidValue(businessObject),
                BusinessObjectInspectionValidator.InspectionIsNotEmpty(inspection),
                BusinessObjectInspectionValidator.InspectionHasKebabCase(inspection),
                BusinessObjectInspectionValidator.InspectionIsNotTooLong(inspection),
                BusinessObjectInspectionValidator.InspectionHasValidValue(inspection));

        private static void EnsureInsertable(BusinessObjectInspection inspection)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return BusinessObjectInspectionValidator.BusinessObjectIsNotEmpty(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectHasKebabCase(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectIsNotTooLong(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectHasValidValue(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectDisplayNameIsNotEmpty(inspection.BusinessObjectDisplayName);
                yield return BusinessObjectInspectionValidator.BusinessObjectDisplayNameIsNotTooLong(inspection.BusinessObjectDisplayName);
                yield return BusinessObjectInspectionValidator.InspectionIsNotEmpty(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionHasKebabCase(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionIsNotTooLong(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionHasValidValue(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionDisplayNameIsNotEmpty(inspection.InspectionDisplayName);
                yield return BusinessObjectInspectionValidator.InspectionDisplayNameIsNotTooLong(inspection.InspectionDisplayName);
                yield return BusinessObjectInspectionValidator.InspectionTextIsNotNull(inspection.InspectionText);
                yield return BusinessObjectInspectionValidator.InspectionTextIsNotTooLong(inspection.InspectionText);
                yield return BusinessObjectInspectionValidator.AssignmentDateIsPositive(inspection.AssignmentDate);
                yield return BusinessObjectInspectionValidator.AssignmentTimeIsInDayTimeRange(inspection.AssignmentTime);

                yield return BusinessObjectInspectionAuditValidator.InspectorIsNotNull(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.InspectorHasKebabCase(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.InspectorIsNotTooLong(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.InspectorHasValidValue(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.AnnotationIsNotNull(inspection.Audit.Annotation);
                yield return BusinessObjectInspectionAuditValidator.AnnotationIsNotTooLong(inspection.Audit.Annotation);
                yield return BusinessObjectInspectionAuditValidator.ResultIsNotNull(inspection.Audit.Result);
                yield return BusinessObjectInspectionAuditValidator.ResultHasValidValue(inspection.Audit.Result);
                yield return BusinessObjectInspectionAuditValidator.AuditDateIsPositive(inspection.Audit.AuditDate);
                yield return BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(inspection.Audit.AuditTime);

                yield return BusinessObjectInspectionAuditScheduleValidator.ThresholdIsInDayTimeRange(inspection.AuditSchedule.Threshold);

                foreach (var expression in inspection.AuditSchedule.Expressions)
                {
                    yield return BusinessObjectInspectionAuditScheduleExpressionValidator.CronExpressionIsNotEmpty(expression.CronExpression);
                    yield return BusinessObjectInspectionAuditScheduleExpressionValidator.CronExpressionIsCronExpression(expression.CronExpression);
                }

                yield return BusinessObjectInspectionAuditScheduleValidator.OmissionsUnique(inspection.AuditSchedule.Omissions);

                foreach (var omission in inspection.AuditSchedule.Omissions)
                {
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditDateIsPositive(omission.PlannedAuditDate);
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditTimeIsInDayTimeRange(omission.PlannedAuditTime);
                }

                yield return BusinessObjectInspectionAuditScheduleValidator.AdditionalsUnique(inspection.AuditSchedule.Additionals);

                foreach (var additional in inspection.AuditSchedule.Additionals)
                {
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditDateIsPositive(additional.PlannedAuditDate);
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditTimeIsInDayTimeRange(additional.PlannedAuditTime);
                }

                yield return BusinessObjectInspectionAuditScheduleValidator.AdditionalsUnique(inspection.AuditSchedule.Appointments);

                foreach (var appointment in inspection.AuditSchedule.Appointments)
                {
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditDateIsPositive(appointment.PlannedAuditDate);
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditTimeIsInDayTimeRange(appointment.PlannedAuditTime);
                }

            }

            Validator.Ensure($"business object with id '{inspection.BusinessObject}/{inspection.Inspection}'", Ensurences());
        }

        private static void EnsureUpdateable(BusinessObjectInspection inspection)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return BusinessObjectInspectionValidator.BusinessObjectIsNotEmpty(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectHasKebabCase(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectIsNotTooLong(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectHasValidValue(inspection.BusinessObject);
                yield return BusinessObjectInspectionValidator.BusinessObjectDisplayNameIsNotEmpty(inspection.BusinessObjectDisplayName);
                yield return BusinessObjectInspectionValidator.BusinessObjectDisplayNameIsNotTooLong(inspection.BusinessObjectDisplayName);
                yield return BusinessObjectInspectionValidator.InspectionIsNotEmpty(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionHasKebabCase(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionIsNotTooLong(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionHasValidValue(inspection.Inspection);
                yield return BusinessObjectInspectionValidator.InspectionDisplayNameIsNotEmpty(inspection.InspectionDisplayName);
                yield return BusinessObjectInspectionValidator.InspectionDisplayNameIsNotTooLong(inspection.InspectionDisplayName);
                yield return BusinessObjectInspectionValidator.InspectionTextIsNotNull(inspection.InspectionText);
                yield return BusinessObjectInspectionValidator.InspectionTextIsNotTooLong(inspection.InspectionText);
                yield return BusinessObjectInspectionValidator.AssignmentDateIsPositive(inspection.AssignmentDate);
                yield return BusinessObjectInspectionValidator.AssignmentTimeIsInDayTimeRange(inspection.AssignmentTime);

                yield return BusinessObjectInspectionAuditValidator.InspectorIsNotNull(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.InspectorHasKebabCase(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.InspectorIsNotTooLong(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.InspectorHasValidValue(inspection.Audit.Inspector);
                yield return BusinessObjectInspectionAuditValidator.AnnotationIsNotNull(inspection.Audit.Annotation);
                yield return BusinessObjectInspectionAuditValidator.AnnotationIsNotTooLong(inspection.Audit.Annotation);
                yield return BusinessObjectInspectionAuditValidator.ResultIsNotNull(inspection.Audit.Result);
                yield return BusinessObjectInspectionAuditValidator.ResultHasValidValue(inspection.Audit.Result);
                yield return BusinessObjectInspectionAuditValidator.AuditDateIsPositive(inspection.Audit.AuditDate);
                yield return BusinessObjectInspectionAuditValidator.AuditTimeIsInDayTimeRange(inspection.Audit.AuditTime);

                yield return BusinessObjectInspectionAuditScheduleValidator.ThresholdIsInDayTimeRange(inspection.AuditSchedule.Threshold);

                foreach (var expression in inspection.AuditSchedule.Expressions)
                {
                    yield return BusinessObjectInspectionAuditScheduleExpressionValidator.CronExpressionIsNotEmpty(expression.CronExpression);
                    yield return BusinessObjectInspectionAuditScheduleExpressionValidator.CronExpressionIsCronExpression(expression.CronExpression);
                }

                yield return BusinessObjectInspectionAuditScheduleValidator.OmissionsUnique(inspection.AuditSchedule.Omissions);

                foreach (var omission in inspection.AuditSchedule.Omissions)
                {
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditDateIsPositive(omission.PlannedAuditDate);
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditTimeIsInDayTimeRange(omission.PlannedAuditTime);
                }

                yield return BusinessObjectInspectionAuditScheduleValidator.AdditionalsUnique(inspection.AuditSchedule.Additionals);

                foreach (var additional in inspection.AuditSchedule.Additionals)
                {
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditDateIsPositive(additional.PlannedAuditDate);
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditTimeIsInDayTimeRange(additional.PlannedAuditTime);
                }

                yield return BusinessObjectInspectionAuditScheduleValidator.AdditionalsUnique(inspection.AuditSchedule.Appointments);

                foreach (var appointment in inspection.AuditSchedule.Appointments)
                {
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditDateIsPositive(appointment.PlannedAuditDate);
                    yield return BusinessObjectInspectionAuditScheduleTimestampValidator.PlannedAuditTimeIsInDayTimeRange(appointment.PlannedAuditTime);
                }

            }

            Validator.Ensure($"business object with id '{inspection.BusinessObject}/{inspection.Inspection}'", Ensurences());
        }

        private static void EnsureDeletable(BusinessObjectInspection inspection)
            => Validator.Ensure($"id '{inspection.BusinessObject}/{inspection.Inspection}' of business object inspection",
                BusinessObjectInspectionValidator.BusinessObjectIsNotEmpty(inspection.BusinessObject),
                BusinessObjectInspectionValidator.BusinessObjectHasKebabCase(inspection.BusinessObject),
                BusinessObjectInspectionValidator.BusinessObjectIsNotTooLong(inspection.BusinessObject),
                BusinessObjectInspectionValidator.BusinessObjectHasValidValue(inspection.BusinessObject),
                BusinessObjectInspectionValidator.InspectionIsNotEmpty(inspection.Inspection),
                BusinessObjectInspectionValidator.InspectionHasKebabCase(inspection.Inspection),
                BusinessObjectInspectionValidator.InspectionIsNotTooLong(inspection.Inspection),
                BusinessObjectInspectionValidator.InspectionHasValidValue(inspection.Inspection));
    }
}