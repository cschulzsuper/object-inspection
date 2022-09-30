using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IOrganizationManager
{
    Organization Get(string organization);
    ValueTask<Organization> GetAsync(string organization);
    IQueryable<Organization> GetQueryable();
    IAsyncEnumerable<Organization> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Organization>, IQueryable<TResult>> query);
    ValueTask InsertAsync(Organization organization);
    ValueTask UpdateAsync(Organization organization);
    ValueTask DeleteAsync(Organization organization);
}