using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
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