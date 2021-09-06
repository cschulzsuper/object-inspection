﻿using Super.Paula.Aggregates.Organizations;

namespace Super.Paula.Management.Contract
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