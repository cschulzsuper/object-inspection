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

        private void EnsureGetable(string businessObject)
            => Validator.Ensure(
                BusinessObjectValidator.UniqueNameHasValue(businessObject),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject));

        private void EnsureInsertable(BusinessObject businessObject)
        {
            IEnumerable<(Func<bool, bool> assertion, Func<FormattableString> messsage)> Ensurences()
            {
                yield return BusinessObjectValidator.UniqueNameHasValue(businessObject.UniqueName);
                yield return BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName);
                yield return BusinessObjectValidator.DisplayNameIsNotEmpty(businessObject);
                yield return BusinessObjectValidator.InspectorIsNotNull(businessObject);
                yield return BusinessObjectValidator.InspectorHasKebabCase(businessObject);

                foreach(var inspection in businessObject.Inspections )
                {
                    yield return BusinessObjectValidator.InspectionUniqueNameHasValue(inspection);
                    yield return BusinessObjectValidator.InspectionUniqueNameHasKebabCase(inspection);
                    yield return BusinessObjectValidator.InspectionDisplayNameIsNotEmpty(inspection);
                    yield return BusinessObjectValidator.InspectionTextIsNotNull(inspection);
                    yield return BusinessObjectValidator.InspectionAuditInspectorIsNotNull(inspection);
                    yield return BusinessObjectValidator.InspectionAuditInspectorHasKebabCase(inspection);
                    yield return BusinessObjectValidator.InspectionAuditAnnotationIsNotNull(inspection);
                    yield return BusinessObjectValidator.InspectionAuditResultHasValidValue(inspection);
                    yield return BusinessObjectValidator.InspectionAuditDateIsPositive(inspection);
                    yield return BusinessObjectValidator.InspectionAuditTimeIsInDayTimeRange(inspection);
                };
            };

            Validator.Ensure(Ensurences());
        }

        private void EnsureUpdateable(BusinessObject businessObject)
        {
            IEnumerable<(Func<bool, bool> assertion, Func<FormattableString> messsage)> Ensurences()
            {
                yield return BusinessObjectValidator.UniqueNameHasValue(businessObject.UniqueName);
                yield return BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName);
                yield return BusinessObjectValidator.DisplayNameIsNotEmpty(businessObject);
                yield return BusinessObjectValidator.InspectorIsNotNull(businessObject);
                yield return BusinessObjectValidator.InspectorHasKebabCase(businessObject);

                foreach (var inspection in businessObject.Inspections)
                {
                    yield return BusinessObjectValidator.InspectionUniqueNameHasValue(inspection);
                    yield return BusinessObjectValidator.InspectionUniqueNameHasKebabCase(inspection);
                    yield return BusinessObjectValidator.InspectionDisplayNameIsNotEmpty(inspection);
                    yield return BusinessObjectValidator.InspectionTextIsNotNull(inspection);
                    yield return BusinessObjectValidator.InspectionAuditInspectorIsNotNull(inspection);
                    yield return BusinessObjectValidator.InspectionAuditInspectorHasKebabCase(inspection);
                    yield return BusinessObjectValidator.InspectionAuditAnnotationIsNotNull(inspection);
                    yield return BusinessObjectValidator.InspectionAuditResultHasValidValue(inspection);
                    yield return BusinessObjectValidator.InspectionAuditDateIsPositive(inspection);
                    yield return BusinessObjectValidator.InspectionAuditTimeIsInDayTimeRange(inspection);
                }
            };

            Validator.Ensure(Ensurences());
        }

        private void EnsureDeleteable(BusinessObject businessObject)
            => Validator.Ensure(
                BusinessObjectValidator.UniqueNameHasValue(businessObject.UniqueName),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName));
    }
}