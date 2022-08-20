using Super.Paula.Data;
using Super.Paula.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public class IdentityInspectorManager : IIdentityInspectorManager
{
    private readonly IRepository<IdentityInspector> _identityInspectorRepository;

    public IdentityInspectorManager(IRepository<IdentityInspector> identityInspectorRepository)
    {
        _identityInspectorRepository = identityInspectorRepository;
    }

    public async ValueTask<IdentityInspector> GetAsync(string identity, string organization, string inspector)
    {
        EnsureGetable(identity, organization, inspector);

        var entity = await _identityInspectorRepository.GetByIdsOrDefaultAsync(identity, organization, inspector);
        if (entity == null)
        {
            throw new ManagementException($"Identity inspector '{identity}/{organization}/{inspector}' was not found.");
        }

        return entity;
    }

    public async ValueTask InsertAsync(IdentityInspector identity)
    {
        EnsureInsertable(identity);

        try
        {
            await _identityInspectorRepository.InsertAsync(identity);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not insert inspector identity '{identity.UniqueName}'.", exception);
        }
    }

    public async ValueTask UpdateAsync(IdentityInspector identity)
    {
        EnsureUpdateable(identity);

        try
        {
            await _identityInspectorRepository.UpdateAsync(identity);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not update inspector identity '{identity.UniqueName}'.", exception);
        }
    }

    public async ValueTask DeleteAsync(IdentityInspector identity)
    {
        EnsureDeletable(identity);

        try
        {
            await _identityInspectorRepository.DeleteAsync(identity);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not delete inspector identity '{identity.UniqueName}'.", exception);
        }
    }

    public IQueryable<IdentityInspector> GetQueryable()
        => _identityInspectorRepository.GetQueryable();

    public IAsyncEnumerable<IdentityInspector> GetAsyncEnumerable()
        => _identityInspectorRepository.GetAsyncEnumerable();

    public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<IdentityInspector>, IQueryable<TResult>> query)
        => _identityInspectorRepository.GetAsyncEnumerable(query);

    public IQueryable<IdentityInspector> GetIdentityBasedQueryable(string identity)
    {
        EnsureGetableIdentityBased(identity);
        return _identityInspectorRepository.GetPartitionQueryable(identity);
    }

    public IAsyncEnumerable<IdentityInspector> GetIdentityBasedAsyncEnumerable(string identity)
    {
        EnsureGetableIdentityBased(identity);
        return _identityInspectorRepository.GetPartitionAsyncEnumerable(identity);
    }

    public IAsyncEnumerable<TResult> GetIdentityBasedAsyncEnumerable<TResult>(string identity, Func<IQueryable<IdentityInspector>, IQueryable<TResult>> query)
    {
        EnsureGetableIdentityBased(identity);
        return _identityInspectorRepository.GetPartitionAsyncEnumerable(query, identity);
    }

    private static void EnsureGetable(string identity, string organization, string inspector)
        => Validator.Ensure($"unique name '{identity}' of inspector identity",
            IdentityInspectorValidator.UniqueNameIsNotEmpty(identity),
            IdentityInspectorValidator.UniqueNameHasKebabCase(identity),
            IdentityInspectorValidator.UniqueNameIsNotTooLong(identity),
            IdentityInspectorValidator.UniqueNameHasValidValue(identity),
            IdentityInspectorValidator.OrganizationIsNotEmpty(organization),
            IdentityInspectorValidator.OrganizationHasKebabCase(organization),
            IdentityInspectorValidator.OrganizationIsNotTooLong(organization),
            IdentityInspectorValidator.OrganizationHasValidValue(identity),
            IdentityInspectorValidator.InspectorIsNotEmpty(inspector),
            IdentityInspectorValidator.InspectorHasKebabCase(inspector),
            IdentityInspectorValidator.InspectorIsNotTooLong(inspector),
            IdentityInspectorValidator.InspectorHasValidValue(inspector));

    private static void EnsureGetableIdentityBased(string identity)
        => Validator.Ensure($"identity for identity inspectors",
            IdentityInspectorValidator.UniqueNameIsNotEmpty(identity),
            IdentityInspectorValidator.UniqueNameHasKebabCase(identity),
            IdentityInspectorValidator.UniqueNameIsNotTooLong(identity));

    private static void EnsureInsertable(IdentityInspector identity)
        => Validator.Ensure($"identity inspector with unique name '{identity.UniqueName}'",
            IdentityInspectorValidator.UniqueNameIsNotEmpty(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameHasKebabCase(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameIsNotTooLong(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameHasValidValue(identity.UniqueName),
            IdentityInspectorValidator.OrganizationIsNotEmpty(identity.Organization),
            IdentityInspectorValidator.OrganizationHasKebabCase(identity.Organization),
            IdentityInspectorValidator.OrganizationIsNotTooLong(identity.Organization),
            IdentityInspectorValidator.OrganizationHasValidValue(identity.Organization),
            IdentityInspectorValidator.InspectorIsNotEmpty(identity.Inspector),
            IdentityInspectorValidator.InspectorHasKebabCase(identity.Inspector),
            IdentityInspectorValidator.InspectorIsNotTooLong(identity.Inspector),
            IdentityInspectorValidator.InspectorHasValidValue(identity.Inspector));

    private static void EnsureUpdateable(IdentityInspector identity)
        => Validator.Ensure($"identity inspector with unique name '{identity.UniqueName}'",
            IdentityInspectorValidator.UniqueNameIsNotEmpty(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameHasKebabCase(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameIsNotTooLong(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameHasValidValue(identity.UniqueName),
            IdentityInspectorValidator.OrganizationIsNotEmpty(identity.Organization),
            IdentityInspectorValidator.OrganizationHasKebabCase(identity.Organization),
            IdentityInspectorValidator.OrganizationIsNotTooLong(identity.Organization),
            IdentityInspectorValidator.OrganizationHasValidValue(identity.Organization),
            IdentityInspectorValidator.InspectorIsNotEmpty(identity.Inspector),
            IdentityInspectorValidator.InspectorHasKebabCase(identity.Inspector),
            IdentityInspectorValidator.InspectorIsNotTooLong(identity.Inspector),
            IdentityInspectorValidator.InspectorHasValidValue(identity.Inspector));

    private static void EnsureDeletable(IdentityInspector identity)
        => Validator.Ensure($"identity inspector with unique name '{identity.UniqueName}'",
            IdentityInspectorValidator.UniqueNameIsNotEmpty(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameHasKebabCase(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameIsNotTooLong(identity.UniqueName),
            IdentityInspectorValidator.UniqueNameHasValidValue(identity.UniqueName));
}