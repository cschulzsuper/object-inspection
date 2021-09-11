using Super.Paula.Aggregates.Administration;

namespace Super.Paula.Management.Administration
{
    public interface IOrganizationManager
    {
        ValueTask<Organization> GetAsync(string organization);
        IQueryable<Organization> GetQueryable();
        IAsyncEnumerable<Organization> GetAsyncEnumerable();
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Organization>, IQueryable<TResult>> query);
        ValueTask InsertAsync(Organization organization);
        ValueTask UpdateAsync(Organization organization);
        ValueTask DeleteAsync(Organization organization);
    }
}