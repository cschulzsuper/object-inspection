using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public interface IInspectorManager
{
    ValueTask<Inspector> GetAsync(string inspector);
    IQueryable<Inspector> GetQueryable();
    IQueryable<Inspector> GetQueryableWhereBusinessObject(string businessObject);
    IAsyncEnumerable<Inspector> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspector>, IQueryable<TResult>> query);
    ValueTask InsertAsync(Inspector inspector);
    ValueTask UpdateAsync(Inspector inspector);
    ValueTask DeleteAsync(Inspector inspector);
}