using Super.Paula.Validation;

namespace Super.Paula.Inventory
{
    public class BusinessObjectManager : IBusinessObjectManager
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
        {
            IEnumerable<(Func<bool, bool> assertion, Func<FormattableString> messsage)> Ensurences()
            {
                yield return BusinessObjectValidator.UniqueNameHasValue(businessObject);
                yield return BusinessObjectValidator.UniqueNameHasKebabCase(businessObject);
                yield return BusinessObjectValidator.UniqueNameIsUnqiue(businessObject, GetQueryable());
                yield return BusinessObjectValidator.DisplayNameHasValue(businessObject);
                yield return BusinessObjectValidator.InspectorIsNotNull(businessObject);
                yield return BusinessObjectValidator.InspectorHasKebabCase(businessObject);

                foreach(var inspection in businessObject.Inspections )
                {
                    yield return BusinessObjectValidator.InspectionUniqueNameHasValue(inspection);
                    yield return BusinessObjectValidator.InspectionUniqueNameHasKebabCase(inspection);
                    yield return BusinessObjectValidator.InspectionUniqueNameIsUnqiue(inspection, businessObject);
                    yield return BusinessObjectValidator.InspectionDisplayNameHasValue(inspection);
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

        private void EnsureUpdateable(BusinessObject businessObject)
        {
            IEnumerable<(Func<bool, bool> assertion, Func<FormattableString> messsage)> Ensurences()
            {
                yield return BusinessObjectValidator.UniqueNameHasValue(businessObject);
                yield return BusinessObjectValidator.UniqueNameHasKebabCase(businessObject);
                yield return BusinessObjectValidator.UniqueNameExists(businessObject, GetQueryable());
                yield return BusinessObjectValidator.DisplayNameHasValue(businessObject);
                yield return BusinessObjectValidator.InspectorIsNotNull(businessObject);
                yield return BusinessObjectValidator.InspectorHasKebabCase(businessObject);

                foreach (var inspection in businessObject.Inspections)
                {
                    yield return BusinessObjectValidator.InspectionUniqueNameHasValue(inspection);
                    yield return BusinessObjectValidator.InspectionUniqueNameHasKebabCase(inspection);
                    yield return BusinessObjectValidator.InspectionUniqueNameIsUnqiue(inspection, businessObject);
                    yield return BusinessObjectValidator.InspectionDisplayNameHasValue(inspection);
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
                BusinessObjectValidator.UniqueNameHasValue(businessObject),
                BusinessObjectValidator.UniqueNameHasKebabCase(businessObject),
                BusinessObjectValidator.UniqueNameExists(businessObject, GetQueryable()));
    }
}