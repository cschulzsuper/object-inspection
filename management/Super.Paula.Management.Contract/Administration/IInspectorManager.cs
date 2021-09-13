using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Administration
{
    public interface IInspectorManager
    {
        ValueTask<Inspector> GetAsync(string inspector);
        IQueryable<Inspector> GetQueryable();
        IAsyncEnumerable<Inspector> GetAsyncEnumerable();
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspector>, IQueryable<TResult>> query);
        ValueTask InsertAsync(Inspector inspector);
        ValueTask UpdateAsync(Inspector inspector);
        ValueTask DeleteAsync(Inspector inspector);
    }
}