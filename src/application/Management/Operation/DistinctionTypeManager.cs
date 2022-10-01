using ChristianSchulz.ObjectInspection.Data;
using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class DistinctionTypeManager : IDistinctionTypeManager
{
    private readonly IRepository<DistinctionType> _distinctionTypeRepository;

    public DistinctionTypeManager(IRepository<DistinctionType> distinctionTypeRepository)
    {
        _distinctionTypeRepository = distinctionTypeRepository;
    }

    public DistinctionType Get(string uniqueName)
    {
        EnsureGetable(uniqueName);

        var entity = _distinctionTypeRepository.GetByIdsOrDefault(uniqueName);
        if (entity == null)
        {
            throw new ManagementException($"Distinction type for '{uniqueName}' was not found.");
        }

        return entity;
    }

    public async ValueTask<DistinctionType> GetAsync(string uniqueName)
    {
        EnsureGetable(uniqueName);

        var entity = await _distinctionTypeRepository.GetByIdsOrDefaultAsync(uniqueName);
        if (entity == null)
        {
            throw new ManagementException($"Distinction type for '{uniqueName}' was not found.");
        }

        return entity;
    }

    public async ValueTask InsertAsync(DistinctionType distinctionType)
    {
        EnsureInsertable(distinctionType);

        try
        {
            await _distinctionTypeRepository.InsertAsync(distinctionType);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not insert distinction type for '{distinctionType.UniqueName}'.", exception);
        }
    }

    public async ValueTask UpdateAsync(DistinctionType distinctionType)
    {
        EnsureUpdateable(distinctionType);

        try
        {
            await _distinctionTypeRepository.UpdateAsync(distinctionType);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not update distinction type for '{distinctionType.UniqueName}'.", exception);
        }
    }

    public async ValueTask DeleteAsync(DistinctionType distinctionType)
    {
        EnsureDeletable(distinctionType);

        try
        {
            await _distinctionTypeRepository.DeleteAsync(distinctionType);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not delete distinction tType for '{distinctionType.UniqueName}'.", exception);
        }
    }

    public IQueryable<DistinctionType> GetQueryable()
        => _distinctionTypeRepository.GetQueryable();

    public IAsyncEnumerable<DistinctionType> GetAsyncEnumerable()
        => _distinctionTypeRepository.GetAsyncEnumerable();

    public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<DistinctionType>, IQueryable<TResult>> query)
        => _distinctionTypeRepository.GetAsyncEnumerable(query);

    private static void EnsureGetable(string distinctionType)
        => Validator.Ensure($"unique name '{distinctionType}' of distinction type",
            DistinctionTypeValidator.UniqueNameIsNotEmpty(distinctionType),
            DistinctionTypeValidator.UniqueNameHasKebabCase(distinctionType),
            DistinctionTypeValidator.UniqueNameIsNotTooLong(distinctionType),
            DistinctionTypeValidator.UniqueNameHasValidValue(distinctionType));

    private static void EnsureInsertable(DistinctionType distinctionType)
    {
        IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
        {
            yield return DistinctionTypeValidator.UniqueNameIsNotEmpty(distinctionType.UniqueName);
            yield return DistinctionTypeValidator.UniqueNameHasKebabCase(distinctionType.UniqueName);
            yield return DistinctionTypeValidator.UniqueNameIsNotTooLong(distinctionType.UniqueName);
            yield return DistinctionTypeValidator.UniqueNameHasValidValue(distinctionType.UniqueName);

            yield return DistinctionTypeValidator.FieldExtensionFieldsAreUnique(distinctionType.Fields);

            foreach (var distinctionTypeField in distinctionType.Fields)
            {
                yield return DistinctionTypeFieldValidator.ExtensionFieldIsNotEmpty(distinctionTypeField.ExtensionField);
                yield return DistinctionTypeFieldValidator.ExtensionFieldHasKebabCase(distinctionTypeField.ExtensionField);
                yield return DistinctionTypeFieldValidator.ExtensionFieldIsNotTooLong(distinctionTypeField.ExtensionField);
                yield return DistinctionTypeFieldValidator.ExtensionFieldHasValidValue(distinctionTypeField.ExtensionField);
            }
        }

        Validator.Ensure($"distinction type with unique name '{distinctionType.UniqueName}'", Ensurences());
    }

    private static void EnsureUpdateable(DistinctionType distinctionType)
    {
        IEnumerable<(bool, Func<(string, FormattableString)>)> Ensurences()
        {
            yield return DistinctionTypeValidator.UniqueNameIsNotEmpty(distinctionType.UniqueName);
            yield return DistinctionTypeValidator.UniqueNameHasKebabCase(distinctionType.UniqueName);
            yield return DistinctionTypeValidator.UniqueNameIsNotTooLong(distinctionType.UniqueName);
            yield return DistinctionTypeValidator.UniqueNameHasValidValue(distinctionType.UniqueName);

            yield return DistinctionTypeValidator.FieldExtensionFieldsAreUnique(distinctionType.Fields);

            foreach (var distinctionTypeField in distinctionType.Fields)
            {
                yield return DistinctionTypeFieldValidator.ExtensionFieldIsNotEmpty(distinctionTypeField.ExtensionField);
                yield return DistinctionTypeFieldValidator.ExtensionFieldHasKebabCase(distinctionTypeField.ExtensionField);
                yield return DistinctionTypeFieldValidator.ExtensionFieldIsNotTooLong(distinctionTypeField.ExtensionField);
                yield return DistinctionTypeFieldValidator.ExtensionFieldHasValidValue(distinctionTypeField.ExtensionField);
            }
        }

        Validator.Ensure($"distinction type with unique name '{distinctionType.UniqueName}'", Ensurences());
    }
    private static void EnsureDeletable(DistinctionType distinctionType)
        => Validator.Ensure($"distinction type with unique name '{distinctionType.UniqueName}'",
            DistinctionTypeValidator.UniqueNameIsNotEmpty(distinctionType.UniqueName),
            DistinctionTypeValidator.UniqueNameHasKebabCase(distinctionType.UniqueName),
            DistinctionTypeValidator.UniqueNameIsNotTooLong(distinctionType.UniqueName),
            DistinctionTypeValidator.UniqueNameHasValidValue(distinctionType.UniqueName));
}