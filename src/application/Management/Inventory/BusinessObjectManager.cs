using Super.Paula.Data;
using Super.Paula.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectManager : IBusinessObjectManager
    {
        private readonly IRepository<BusinessObject> _businessObjectRepository;

        public BusinessObjectManager(IRepository<BusinessObject> businessObjectRepository)
        {
            _businessObjectRepository = businessObjectRepository;
        }

        public async ValueTask<BusinessObject> GetAsync(string businessObject)
        {
            EnsureGetable(businessObject);

            var entity = await _businessObjectRepository.GetByIdsOrDefaultAsync(businessObject);
            if (entity == null)
            {
                throw new ManagementException($"Business object '{businessObject}' was not found");
            }

            return entity;
        }

        public IQueryable<BusinessObject> GetQueryable()
            => _businessObjectRepository.GetPartitionQueryable();

        public IQueryable<BusinessObject> GetQueryableWhereInspection(string inspection)
            => _businessObjectRepository.GetPartitionQueryable(
                $"SELECT * FROM c WHERE ARRAY_CONTAINS(c.Inspections, {{\"UniqueName\": {inspection}}}, true)");

        public IAsyncEnumerable<BusinessObject> GetAsyncEnumerable()
            => _businessObjectRepository.GetPartitionAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObject>, IQueryable<TResult>> query)
            => _businessObjectRepository.GetPartitionAsyncEnumerable(query);

        public async ValueTask InsertAsync(BusinessObject businessObject)
        {
            EnsureInsertable(businessObject);

            try
            {
                await _businessObjectRepository.InsertAsync(businessObject);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not insert business object '{businessObject.UniqueName}'", exception);
            }
        }

        public async ValueTask UpdateAsync(BusinessObject businessObject)
        {
            EnsureUpdateable(businessObject);

            try
            {
                await _businessObjectRepository.UpdateAsync(businessObject);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not update business object '{businessObject.UniqueName}'", exception);
            }
        }

        public async ValueTask DeleteAsync(BusinessObject businessObject)
        {
            EnsureDeletable(businessObject);

            try
            {
                await _businessObjectRepository.DeleteAsync(businessObject);
            }
            catch (Exception exception)
            {
                throw new ManagementException($"Could not delete business object '{businessObject.UniqueName}'", exception);
            }
        }

        private static void EnsureGetable(string businessObject)
            => Validator.Ensure($"unique name '{businessObject}' of business object",
                BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject),
                BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject));

        private static void EnsureInsertable(BusinessObject businessObject)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject.UniqueName);
                yield return BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName);
                yield return BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject.UniqueName);
                yield return BusinessObjectValidator.DisplayNameIsNotEmpty(businessObject.DisplayName);
                yield return BusinessObjectValidator.DisplayNameIsNotTooLong(businessObject.DisplayName);
                yield return BusinessObjectValidator.InspectorIsNotNull(businessObject.Inspector);
                yield return BusinessObjectValidator.InspectorHasKebabCase(businessObject.Inspector);
                yield return BusinessObjectValidator.InspectorIsNotTooLong(businessObject.Inspector);

                foreach (var inspection in businessObject.Inspections)
                {
                    yield return BusinessObjectInspectionValidator.UniqueNameIsNotEmpty(inspection.UniqueName);
                    yield return BusinessObjectInspectionValidator.UniqueNameHasKebabCase(inspection.UniqueName);
                    yield return BusinessObjectInspectionValidator.UniqueNameIsNotTooLong(inspection.UniqueName);
                    yield return BusinessObjectInspectionValidator.DisplayNameIsNotEmpty(inspection.DisplayName);
                    yield return BusinessObjectInspectionValidator.DisplayNameIsNotTooLong(inspection.DisplayName);
                    yield return BusinessObjectInspectionValidator.TextIsNotNull(inspection.Text);
                    yield return BusinessObjectInspectionValidator.TextIsNotTooLong(inspection.Text);
                    yield return BusinessObjectInspectionValidator.AuditInspectorIsNotNull(inspection.AuditInspector);
                    yield return BusinessObjectInspectionValidator.AuditInspectorHasKebabCase(inspection.AuditInspector);
                    yield return BusinessObjectInspectionValidator.AuditInspectorIsNotTooLong(inspection.AuditInspector);
                    yield return BusinessObjectInspectionValidator.AuditAnnotationIsNotNull(inspection.AuditAnnotation);
                    yield return BusinessObjectInspectionValidator.AuditAnnotationIsNotTooLong(inspection.AuditAnnotation);
                    yield return BusinessObjectInspectionValidator.AuditResultIsNotNull(inspection.AuditAnnotation);
                    yield return BusinessObjectInspectionValidator.AuditResultHasValidValue(inspection.AuditAnnotation);
                    yield return BusinessObjectInspectionValidator.AuditDateIsPositive(inspection.AuditDate);
                    yield return BusinessObjectInspectionValidator.AuditTimeIsInDayTimeRange(inspection.AuditTime);
                    yield return BusinessObjectInspectionValidator.AssignmentDateIsPositive(inspection.AssignmentDate);
                    yield return BusinessObjectInspectionValidator.AssignmentTimeIsInDayTimeRange(inspection.AssignmentTime);
                    yield return BusinessObjectInspectionValidator.AuditDelayThresholdIsInDayTimeRange(inspection.AuditDelayThreshold);
                    yield return BusinessObjectInspectionValidator.AuditThresholdIsInDayTimeRange(inspection.AuditThreshold);

                    foreach (var auditSchedule in inspection.AuditSchedules)
                    {
                        yield return BusinessObjectInspectionAuditScheduleValidator.CronExpressionIsNotNull(auditSchedule.CronExpression);
                        yield return BusinessObjectInspectionAuditScheduleValidator.CronExpressionIsCronExpression(auditSchedule.CronExpression);
                        yield return BusinessObjectInspectionAuditScheduleValidator.AdjustmentsUnique(auditSchedule.Adjustments);

                        foreach (var adjustment in auditSchedule.Adjustments)
                        {
                            yield return BusinessObjectInspectionAuditScheduleAdjustmentValidator.PostponedAuditDateIsPositive(adjustment.PostponedAuditDate);
                            yield return BusinessObjectInspectionAuditScheduleAdjustmentValidator.PostponedAuditTimeIsInDayTimeRange(adjustment.PostponedAuditTime);
                            yield return BusinessObjectInspectionAuditScheduleAdjustmentValidator.PlannedAuditDateIsPositive(adjustment.PlannedAuditDate);
                            yield return BusinessObjectInspectionAuditScheduleAdjustmentValidator.PlannedAuditTimeIsInDayTimeRange(adjustment.PlannedAuditTime);
                        }
                    }
                }
            }

            Validator.Ensure($"business object with unique name '{businessObject.UniqueName}'", Ensurences());
        }

        private static void EnsureUpdateable(BusinessObject businessObject)
        {
            IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
            {
                yield return BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject.UniqueName);
                yield return BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName);
                yield return BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject.UniqueName);
                yield return BusinessObjectValidator.DisplayNameIsNotEmpty(businessObject.DisplayName);
                yield return BusinessObjectValidator.DisplayNameIsNotTooLong(businessObject.DisplayName);
                yield return BusinessObjectValidator.InspectorIsNotNull(businessObject.Inspector);
                yield return BusinessObjectValidator.InspectorHasKebabCase(businessObject.Inspector);
                yield return BusinessObjectValidator.InspectorIsNotTooLong(businessObject.Inspector);

                foreach (var inspection in businessObject.Inspections)
                {
                    yield return BusinessObjectInspectionValidator.UniqueNameIsNotEmpty(inspection.UniqueName);
                    yield return BusinessObjectInspectionValidator.UniqueNameHasKebabCase(inspection.UniqueName);
                    yield return BusinessObjectInspectionValidator.UniqueNameIsNotTooLong(inspection.UniqueName);
                    yield return BusinessObjectInspectionValidator.DisplayNameIsNotEmpty(inspection.DisplayName);
                    yield return BusinessObjectInspectionValidator.DisplayNameIsNotTooLong(inspection.DisplayName);
                    yield return BusinessObjectInspectionValidator.TextIsNotNull(inspection.Text);
                    yield return BusinessObjectInspectionValidator.TextIsNotTooLong(inspection.Text);
                    yield return BusinessObjectInspectionValidator.AuditInspectorIsNotNull(inspection.AuditInspector);
                    yield return BusinessObjectInspectionValidator.AuditInspectorHasKebabCase(inspection.AuditInspector);
                    yield return BusinessObjectInspectionValidator.AuditInspectorIsNotTooLong(inspection.AuditInspector);
                    yield return BusinessObjectInspectionValidator.AuditAnnotationIsNotNull(inspection.AuditAnnotation);
                    yield return BusinessObjectInspectionValidator.AuditAnnotationIsNotTooLong(inspection.AuditAnnotation);
                    yield return BusinessObjectInspectionValidator.AuditResultIsNotNull(inspection.AuditResult);
                    yield return BusinessObjectInspectionValidator.AuditResultHasValidValue(inspection.AuditResult);
                    yield return BusinessObjectInspectionValidator.AuditDateIsPositive(inspection.AuditDate);
                    yield return BusinessObjectInspectionValidator.AuditTimeIsInDayTimeRange(inspection.AuditTime);
                    yield return BusinessObjectInspectionValidator.AssignmentDateIsPositive(inspection.AssignmentDate);
                    yield return BusinessObjectInspectionValidator.AssignmentTimeIsInDayTimeRange(inspection.AssignmentTime);
                    yield return BusinessObjectInspectionValidator.AuditDelayThresholdIsInDayTimeRange(inspection.AuditDelayThreshold);
                    yield return BusinessObjectInspectionValidator.AuditThresholdIsInDayTimeRange(inspection.AuditThreshold);

                    foreach (var auditSchedule in inspection.AuditSchedules)
                    {
                        yield return BusinessObjectInspectionAuditScheduleValidator.CronExpressionIsNotNull(auditSchedule.CronExpression);
                        yield return BusinessObjectInspectionAuditScheduleValidator.CronExpressionIsCronExpression(auditSchedule.CronExpression);
                    }
                }
            }

            Validator.Ensure($"business object with unique name '{businessObject.UniqueName}'", Ensurences());
        }

        private static void EnsureDeletable(BusinessObject businessObject)
            => Validator.Ensure($"business object with unique name '{businessObject.UniqueName}'",
                BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject.UniqueName),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName),
                BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject.UniqueName));
    }
}