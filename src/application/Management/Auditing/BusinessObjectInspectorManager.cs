using ChristianSchulz.ObjectInspection.Data;
using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspectorManager : IBusinessObjectInspectorManager
{
    private readonly IRepository<BusinessObjectInspector> _repository;

    public BusinessObjectInspectorManager(IRepository<BusinessObjectInspector> repository)
    {
        _repository = repository;
    }

    public async ValueTask<BusinessObjectInspector> GetAsync(string businessObject, string inspector)
    {
        EnsureGetable(businessObject, inspector);

        var entity = await _repository.GetByIdsOrDefaultAsync(businessObject, inspector);
        if (entity == null)
        {
            throw new ManagementException($"Business object inspector '{businessObject}/{inspector}' was not found.");
        }

        return entity;
    }

    public IQueryable<BusinessObjectInspector> GetQueryable()
        => _repository.GetQueryable();

    public IAsyncEnumerable<BusinessObjectInspector> GetAsyncEnumerable()
        => _repository.GetAsyncEnumerable();

    public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObjectInspector>, IQueryable<TResult>> query)
        => _repository.GetAsyncEnumerable(query);

    public async ValueTask InsertAsync(BusinessObjectInspector inspector)
    {
        EnsureInsertable(inspector);

        try
        {
            await _repository.InsertAsync(inspector);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not insert business object inspector '{inspector.BusinessObject}/{inspector.Inspector}'.", exception);
        }
    }

    public async ValueTask UpdateAsync(BusinessObjectInspector inspector)
    {
        EnsureUpdateable(inspector);

        try
        {
            await _repository.UpdateAsync(inspector);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not update business object inspector '{inspector.BusinessObject}/{inspector.Inspector}'.", exception);
        }
    }

    public async ValueTask DeleteAsync(BusinessObjectInspector inspector)
    {
        EnsureDeletable(inspector);

        try
        {
            await _repository.DeleteAsync(inspector);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not delete business object inspector '{inspector.BusinessObject}/{inspector.Inspector}'.", exception);
        }
    }

    private static void EnsureGetable(string businessObject, string inspector)
        => Validator.Ensure($"id '{businessObject}/{inspector}' of business object inspector",
            BusinessObjectInspectorValidator.BusinessObjectIsNotEmpty(businessObject),
            BusinessObjectInspectorValidator.BusinessObjectHasKebabCase(businessObject),
            BusinessObjectInspectorValidator.BusinessObjectIsNotTooLong(businessObject),
            BusinessObjectInspectorValidator.BusinessObjectHasValidValue(businessObject),
            BusinessObjectInspectorValidator.InspectorIsNotEmpty(inspector),
            BusinessObjectInspectorValidator.InspectorHasKebabCase(inspector),
            BusinessObjectInspectorValidator.InspectorIsNotTooLong(inspector),
            BusinessObjectInspectorValidator.InspectorHasValidValue(inspector));

    private static void EnsureInsertable(BusinessObjectInspector inspector)
    {
        IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
        {
            yield return BusinessObjectInspectorValidator.BusinessObjectIsNotEmpty(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectHasKebabCase(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectIsNotTooLong(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectHasValidValue(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectDisplayNameIsNotEmpty(inspector.BusinessObjectDisplayName);
            yield return BusinessObjectInspectorValidator.BusinessObjectDisplayNameIsNotTooLong(inspector.BusinessObjectDisplayName);
            yield return BusinessObjectInspectorValidator.InspectorIsNotEmpty(inspector.Inspector);
            yield return BusinessObjectInspectorValidator.InspectorHasKebabCase(inspector.Inspector);
            yield return BusinessObjectInspectorValidator.InspectorIsNotTooLong(inspector.Inspector);
            yield return BusinessObjectInspectorValidator.InspectorHasValidValue(inspector.Inspector);
        }

        Validator.Ensure($"business object with id '{inspector.BusinessObject}/{inspector.Inspector}'", Ensurences());
    }

    private static void EnsureUpdateable(BusinessObjectInspector inspector)
    {
        IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
        {
            yield return BusinessObjectInspectorValidator.BusinessObjectIsNotEmpty(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectHasKebabCase(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectIsNotTooLong(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectHasValidValue(inspector.BusinessObject);
            yield return BusinessObjectInspectorValidator.BusinessObjectDisplayNameIsNotEmpty(inspector.BusinessObjectDisplayName);
            yield return BusinessObjectInspectorValidator.BusinessObjectDisplayNameIsNotTooLong(inspector.BusinessObjectDisplayName);
            yield return BusinessObjectInspectorValidator.InspectorIsNotEmpty(inspector.Inspector);
            yield return BusinessObjectInspectorValidator.InspectorHasKebabCase(inspector.Inspector);
            yield return BusinessObjectInspectorValidator.InspectorIsNotTooLong(inspector.Inspector);
            yield return BusinessObjectInspectorValidator.InspectorHasValidValue(inspector.Inspector);
        }

        Validator.Ensure($"business object with id '{inspector.BusinessObject}/{inspector.Inspector}'", Ensurences());
    }

    private static void EnsureDeletable(BusinessObjectInspector inspector)
        => Validator.Ensure($"id '{inspector.BusinessObject}/{inspector.Inspector}' of business object inspector",
            BusinessObjectInspectorValidator.BusinessObjectIsNotEmpty(inspector.BusinessObject),
            BusinessObjectInspectorValidator.BusinessObjectHasKebabCase(inspector.BusinessObject),
            BusinessObjectInspectorValidator.BusinessObjectIsNotTooLong(inspector.BusinessObject),
            BusinessObjectInspectorValidator.BusinessObjectHasValidValue(inspector.BusinessObject),
            BusinessObjectInspectorValidator.InspectorIsNotEmpty(inspector.Inspector),
            BusinessObjectInspectorValidator.InspectorHasKebabCase(inspector.Inspector),
            BusinessObjectInspectorValidator.InspectorIsNotTooLong(inspector.Inspector),
            BusinessObjectInspectorValidator.InspectorHasValidValue(inspector.Inspector));
}