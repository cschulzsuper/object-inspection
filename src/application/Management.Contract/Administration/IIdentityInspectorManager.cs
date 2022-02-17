using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IIdentityInspectorManager
    {
        ValueTask<IdentityInspector> GetAsync(string identity, string organization, string inspector);
        IQueryable<IdentityInspector> GetQueryable();
        IAsyncEnumerable<IdentityInspector> GetAsyncEnumerable();
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<IdentityInspector>, IQueryable<TResult>> query);
        IQueryable<IdentityInspector> GetIdentityBasedQueryable(string identity);
        IAsyncEnumerable<IdentityInspector> GetIdentityBasedAsyncEnumerable(string identity);
        IAsyncEnumerable<TResult> GetIdentityBasedAsyncEnumerable<TResult>(string identity, Func<IQueryable<IdentityInspector>, IQueryable<TResult>> query);
        ValueTask InsertAsync(IdentityInspector identity);
        ValueTask UpdateAsync(IdentityInspector identity);
        ValueTask DeleteAsync(IdentityInspector identity);
    }
}