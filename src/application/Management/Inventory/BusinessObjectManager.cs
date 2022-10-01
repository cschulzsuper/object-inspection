using ChristianSchulz.ObjectInspection.Data;
using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

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
            throw new ManagementException($"Business object '{businessObject}' was not found.");
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
            throw new ManagementException($"Could not insert business object '{businessObject.UniqueName}'.", exception);
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
            throw new ManagementException($"Could not update business object '{businessObject.UniqueName}'.", exception);
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
            throw new ManagementException($"Could not delete business object '{businessObject.UniqueName}'.", exception);
        }
    }

    private static void EnsureGetable(string businessObject)
        => Validator.Ensure($"unique name '{businessObject}' of business object",
            BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject),
            BusinessObjectValidator.UniqueNameHasKebabCase(businessObject),
            BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject),
            BusinessObjectValidator.UniqueNameHasValidValue(businessObject));

    private static void EnsureInsertable(BusinessObject businessObject)
    {
        IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
        {
            yield return BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject.UniqueName);
            yield return BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName);
            yield return BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject.UniqueName);
            yield return BusinessObjectValidator.UniqueNameHasValidValue(businessObject.UniqueName);
            yield return BusinessObjectValidator.DistinctionTypeIsNotNull(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DistinctionTypeHasKebabCase(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DistinctionTypeIsNotTooLong(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DistinctionTypeHasValidValue(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DisplayNameIsNotEmpty(businessObject.DisplayName);
            yield return BusinessObjectValidator.DisplayNameIsNotTooLong(businessObject.DisplayName);
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
            yield return BusinessObjectValidator.UniqueNameHasValidValue(businessObject.UniqueName);
            yield return BusinessObjectValidator.DistinctionTypeIsNotNull(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DistinctionTypeHasKebabCase(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DistinctionTypeIsNotTooLong(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DistinctionTypeHasValidValue(businessObject.DistinctionType);
            yield return BusinessObjectValidator.DisplayNameIsNotEmpty(businessObject.DisplayName);
            yield return BusinessObjectValidator.DisplayNameIsNotTooLong(businessObject.DisplayName);
        }

        Validator.Ensure($"business object with unique name '{businessObject.UniqueName}'", Ensurences());
    }

    private static void EnsureDeletable(BusinessObject businessObject)
        => Validator.Ensure($"business object with unique name '{businessObject.UniqueName}'",
            BusinessObjectValidator.UniqueNameIsNotEmpty(businessObject.UniqueName),
            BusinessObjectValidator.UniqueNameHasKebabCase(businessObject.UniqueName),
            BusinessObjectValidator.UniqueNameIsNotTooLong(businessObject.UniqueName),
            BusinessObjectValidator.UniqueNameHasValidValue(businessObject.UniqueName));
}