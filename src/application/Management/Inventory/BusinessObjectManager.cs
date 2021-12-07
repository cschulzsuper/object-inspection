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

        public IQueryable<BusinessObject> GetQueryableWithInspection(string inspection)
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
            EnsureDeleteable(businessObject);

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

                foreach (var inspection in businessObject.Inspections )
                {
                    yield return BusinessObjectValidator.InspectionUniqueNameIsNotEmpty(inspection.UniqueName);
                    yield return BusinessObjectValidator.InspectionUniqueNameHasKebabCase(inspection.UniqueName);
                    yield return BusinessObjectValidator.InspectionUniqueNameIsNotTooLong(inspection.UniqueName);
                    yield return BusinessObjectValidator.InspectionDisplayNameIsNotEmpty(inspection.DisplayName);
                    yield return BusinessObjectValidator.InspectionDisplayNameIsNotTooLong(inspection.DisplayName);
                    yield return BusinessObjectValidator.InspectionTextIsNotNull(inspection.Text);
                    yield return BusinessObjectValidator.InspectionTextIsNotTooLong(inspection.Text);
                    yield return BusinessObjectValidator.InspectionAuditInspectorIsNotNull(inspection.AuditInspector);
                    yield return BusinessObjectValidator.InspectionAuditInspectorHasKebabCase(inspection.AuditInspector);
                    yield return BusinessObjectValidator.InspectionAuditInspectorIsNotTooLong(inspection.AuditInspector);
                    yield return BusinessObjectValidator.InspectionAuditAnnotationIsNotNull(inspection.AuditAnnotation);
                    yield return BusinessObjectValidator.InspectionAuditAnnotationIsNotTooLong(inspection.AuditAnnotation);
                    yield return BusinessObjectValidator.InspectionAuditResultIsNotNull(inspection.AuditAnnotation);
                    yield return BusinessObjectValidator.InspectionAuditResultHasValidValue(inspection.AuditAnnotation);
                    yield return BusinessObjectValidator.InspectionAuditDateIsPositive(inspection.AuditDate);
                    yield return BusinessObjectValidator.InspectionAuditTimeIsInDayTimeRange(inspection.AuditTime);
                };
            };

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
                    yield return BusinessObjectValidator.InspectionUniqueNameIsNotEmpty(inspection.UniqueName);
                    yield return BusinessObjectValidator.InspectionUniqueNameHasKebabCase(inspection.UniqueName);
                    yield return BusinessObjectValidator.InspectionUniqueNameIsNotTooLong(inspection.UniqueName);
                    yield return BusinessObjectValidator.InspectionDisplayNameIsNotEmpty(inspection.DisplayName);
                    yield return BusinessObjectValidator.InspectionDisplayNameIsNotTooLong(inspection.DisplayName);
                    yield return BusinessObjectValidator.InspectionTextIsNotNull(inspection.Text);
                    yield return BusinessObjectValidator.InspectionTextIsNotTooLong(inspection.Text);
                    yield return BusinessObjectValidator.InspectionAuditInspectorIsNotNull(inspection.AuditInspector);
                    yield return BusinessObjectValidator.InspectionAuditInspectorHasKebabCase(inspection.AuditInspector);
                    yield return BusinessObjectValidator.InspectionAuditInspectorIsNotTooLong(inspection.AuditInspector);
                    yield return BusinessObjectValidator.InspectionAuditAnnotationIsNotNull(inspection.AuditAnnotation);
                    yield return BusinessObjectValidator.InspectionAuditAnnotationIsNotTooLong(inspection.AuditAnnotation);
                    yield return BusinessObjectValidator.InspectionAuditResultIsNotNull(inspection.AuditResult);
                    yield return BusinessObjectValidator.InspectionAuditResultHasValidValue(inspection.AuditResult);
                    yield return BusinessObjectValidator.InspectionAuditDateIsPositive(inspection.AuditDate);
                    yield return BusinessObjectValidator.InspectionAuditTimeIsInDayTimeRange(inspection.AuditTime);
                };
            };

            Validator.Ensure($"business object with unique name '{businessObject.UniqueName}'", Ensurences());
        }

        private static void EnsureDeleteable(BusinessObject businessObject)
            => Validator.Ensure($"business object with unique name '{businessObject.UniqueName}'",
                BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject.UniqueName),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName),
                BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject.UniqueName));
    }
}